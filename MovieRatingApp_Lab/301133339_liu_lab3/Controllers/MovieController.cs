using Amazon.S3;
using Amazon.S3.Model;
using _301133339_liu_lab3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime;
using Amazon;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace _301133339_liu_lab3.Controllers
{
    public class MovieController : Controller
    {

        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string? _bucketName;
        private readonly string _moviesTableName = "Movies";
        private readonly ILogger<MovieController> _logger;

        private readonly UserManager<IdentityUser> _userManager; // Assuming ApplicationUser is your user entity


        public MovieController(IAmazonS3 s3Client, IAmazonDynamoDB dynamoDbClient, IConfiguration configuration, ILogger<MovieController> logger, UserManager<IdentityUser> userManager)
        {
            _bucketName = configuration["AWS:BucketName"];

            // Initialize AWS clients with credentials from appsettings.json
            var awsCredentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"]);
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);

            _s3Client = new AmazonS3Client(awsCredentials, region);
            _dynamoDbClient = new AmazonDynamoDBClient(awsCredentials, region);
            _logger = logger;

            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger?.LogWarning("Model state is invalid. Errors: {ModelStateErrors}",
            ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return View(model);
            }
            if (model == null || model.MovieFile == null)
            {
                _logger.LogError("AddMovieViewModel or MovieFile is null.");
                ModelState.AddModelError("", "The movie information is incomplete.");
                return View(model);
            }
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User); // Fetch the logged-in user
                if (user == null)
                {
                    _logger.LogError("User is not found.");
                    ModelState.AddModelError("", "An error occurred while identifying the user. Please try again.");
                    return View(model);
                }
                var keyName = $"{model.Title}_{Guid.NewGuid()}";
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName,
                    InputStream = model.MovieFile.OpenReadStream(),
                    ContentType = model.MovieFile.ContentType,
                    AutoCloseStream = true
                };

                var response = await _s3Client.PutObjectAsync(putRequest);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError("Failed to upload the video to S3 with status code {StatusCode}", response.HttpStatusCode);
                    throw new Exception("Failed to upload the video to S3.");
                }

                var putItemRequest = new PutItemRequest
                {
                    TableName = _moviesTableName,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "movieId", new AttributeValue { S = Guid.NewGuid().ToString() } },
                        { "Title", new AttributeValue { S = model.Title } },
                        { "Directors", new AttributeValue { S = model.DirectorNames } },
                        { "Genre", new AttributeValue { S = model.Genre } },
                        { "ReleaseTime", new AttributeValue { S = model.ReleaseTime.ToString() } },
                        { "S3Key", new AttributeValue { S = keyName }  },
                        { "UserId", new AttributeValue { S = user.Id } }
                    }
                };

                var dynamoResponse = await _dynamoDbClient.PutItemAsync(putItemRequest);

                if (dynamoResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError("Failed to insert movie data into DynamoDB with status code {StatusCode}", dynamoResponse.HttpStatusCode);
                    await _s3Client.DeleteObjectAsync(_bucketName, keyName);
                    throw new Exception("Failed to insert movie data into DynamoDB.");
                }

                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a movie.");
                ModelState.AddModelError("", "An error occurred while adding the movie. Please try again.");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMovie([FromBody] DeleteMovieViewModel model)
        {
            try
            {
                // Delete from S3
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = model.S3Key // You'll need to ensure that S3Key is passed correctly
                };
                await _s3Client.DeleteObjectAsync(deleteObjectRequest);

                // Delete from DynamoDB
                var deleteItemRequest = new DeleteItemRequest
                {
                    TableName = _moviesTableName,
                    Key = new Dictionary<string, AttributeValue>
            {
                { "movieId", new AttributeValue { S = model.MovieId } }
            }
                };
                await _dynamoDbClient.DeleteItemAsync(deleteItemRequest);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a movie.");
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadMovie(string movieId, string s3Key)
        {
            try
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = s3Key
                };

                using (var response = await _s3Client.GetObjectAsync(getRequest))
                {
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var contentType = response.Headers["Content-Type"];
                        // Set the desired filename for the download
                        var filename = s3Key.Split('/').LastOrDefault();

                        // Ensure the stream is disposed once the response is returned
                        using (var stream = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(stream);
                            return File(stream.ToArray(), contentType, filename);
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to download the video from S3 with status code {StatusCode}", response.HttpStatusCode);
                        return View("Error");
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading a movie from S3.");
                return View("Error"); // or return an appropriate error response
            }
        }

        public async Task<Movie?> GetMovieFromDynamoDB(string movieId)
        {
            // Check if movieId is null or empty
            if (string.IsNullOrEmpty(movieId))
            {
                _logger.LogError("MovieId is null or empty.");
                return null;
            }

            var request = new GetItemRequest
            {
                TableName = _moviesTableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "movieId", new AttributeValue { S = movieId } }
                }
                    };

            try
            {
                var response = await _dynamoDbClient.GetItemAsync(request);

                if (response.Item == null || !response.IsItemSet)
                    return null;

                var movie = new Movie
                {
                    movieId = response.Item["movieId"].S,
                    Title = response.Item["Title"].S,
                    DirectorNames = response.Item["Directors"].S, // Make sure to use the correct attribute name from DynamoDB
                    Genre = response.Item["Genre"].S,
                    ReleaseTime = DateTime.Parse(response.Item["ReleaseTime"].S),
                    // Assuming Comments is a list attribute in DynamoDB
                    Comments = response.Item.ContainsKey("Comments") ? response.Item["Comments"].L.Select(c => new Comment
                    {
                        Username = c.M["Username"].S,
                        Text = c.M["Text"].S,
                        Rating = int.Parse(c.M["Rating"].N)
                    }).ToList() : new List<Comment>()
                };

                return movie;
            }
            catch (AmazonDynamoDBException ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching movie with ID {movieId} from DynamoDB.");
                return null;
            }
        }


        [HttpGet]
        public async Task<IActionResult> ModifyMovie(string movieId)
        {
            var movie = await GetMovieFromDynamoDB(movieId);
            if (movie == null)
            {
                return View("Error"); // Movie not found, handle appropriately
            }

            var model = new ModifyMovieViewModel
            {
                MovieId = movie.movieId,
                Title = movie.Title,
                DirectorNames = movie.DirectorNames,
                Genre = movie.Genre,
                ReleaseTime = movie.ReleaseTime,
                S3Key = movie.S3Key
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ModifyMovie(ModifyMovieViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch the existing movie from DynamoDB
            var movie = await GetMovieFromDynamoDB(model.MovieId);
            if (movie == null)
            {
                return View("Error"); // Movie not found, handle appropriately
            }

            // Update movie properties
            movie.Title = model.Title;
            movie.DirectorNames = model.DirectorNames;
            movie.Genre = model.Genre;
            movie.ReleaseTime = model.ReleaseTime.HasValue ? model.ReleaseTime.Value : default(DateTime);


            // If a new file is uploaded, upload it to S3 and update the movie's S3Key
            if (model.MovieFile != null && model.MovieFile.Length > 0)
            {
                var keyName = $"{model.Title}_{Guid.NewGuid()}";
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName,
                    InputStream = model.MovieFile.OpenReadStream(),
                    ContentType = model.MovieFile.ContentType,
                    AutoCloseStream = true
                };

                var response = await _s3Client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    movie.S3Key = keyName;
                }
                else
                {
                    // Handle the error appropriately
                }
            }

            // Update the movie in DynamoDB
            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _moviesTableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "movieId", new AttributeValue { S = movie.movieId } }
                },
                // Add the necessary update expressions for the attributes you want to update
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#T", "Title"},
                    {"#D", "Directors"},
                    {"#G", "Genre"},
                    {"#R", "ReleaseTime"},
                    {"#S", "S3Key"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":t", new AttributeValue { S = movie.Title }},
                    {":d", new AttributeValue { S = movie.DirectorNames }},
                    {":g", new AttributeValue { S = movie.Genre }},
                    {":r", new AttributeValue { S = movie.ReleaseTime.ToString() }},
                    {":s", new AttributeValue { S = movie.S3Key }}
                },
                UpdateExpression = "SET #T = :t, #D = :d, #G = :g, #R = :r, #S = :s"
            };

            var dynamoResponse = await _dynamoDbClient.UpdateItemAsync(updateItemRequest);
            if (dynamoResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                // Handle the error appropriately
            }

            // Redirect to the Dashboard or another appropriate page
            return RedirectToAction("Dashboard", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Comment(string movieId)
        {
            _logger.LogInformation($"Received movieId for comment: {movieId}"); // Log the movieId

            var movie = await GetMovieFromDynamoDB(movieId);
            if (movie == null)
            {
                _logger.LogError($"Movie not found for movieId: {movieId}"); // Log if the movie is not found

                return View("Error"); // Handle appropriately
            }

            var model = new CommentViewModel
            {
                MovieId = movie.movieId,
                Title = movie.Title,
                DirectorNames = movie.DirectorNames,
                Genre = movie.Genre,
                ReleaseTime = movie.ReleaseTime,
                Comments = movie.Comments
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            _logger.LogInformation($"MovieId: {model.MovieId}");
            _logger.LogInformation("Entering Comment action method.");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid. MovieId: {MovieId}", model.MovieId);
                // Code to add the comment to the DynamoDB table

                // Log success message
                _logger.LogInformation("Comment added successfully for MovieId: {MovieId}", model.MovieId);
            }
            else
            {
                // Log model state errors
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var value = ViewData.ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        _logger.LogError("Error in model state. Key: {Key}, Error: {Error}", modelStateKey, error.ErrorMessage);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return View("Comment",model);
            }
            if (string.IsNullOrEmpty(model.NewComment.Username) ||
      string.IsNullOrEmpty(model.NewComment.Text) ||
      model.NewComment.Rating <= 0)
            {
                ModelState.AddModelError("", "Comment details are incomplete.");
                return View(model);
            }

            var movie = await GetMovieFromDynamoDB(model.MovieId);
            if (movie == null)
            {
                return View("Error"); // Handle appropriately
            }

            movie.Comments.Add(model.NewComment);

            // Convert the comments list to a suitable format for DynamoDB (List of Maps)
            var commentsAttribute = new List<AttributeValue>();
            foreach (var comment in movie.Comments)
            {
                commentsAttribute.Add(new AttributeValue
                {
                    M = new Dictionary<string, AttributeValue>
            {
                { "Username", new AttributeValue { S = comment.Username } },
                { "Text", new AttributeValue { S = comment.Text } },
                { "Rating", new AttributeValue { N = comment.Rating.ToString() } }
            }
                });
            }

            // Update the movie in DynamoDB to include the new comment
            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _moviesTableName,
                Key = new Dictionary<string, AttributeValue>
        {
            { "movieId", new AttributeValue { S = movie.movieId } }
        },
                ExpressionAttributeNames = new Dictionary<string, string>
        {
            {"#C", "Comments"}
        },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        {
            {":c", new AttributeValue { L = commentsAttribute }}
        },
                UpdateExpression = "SET #C = :c"
            };

            var dynamoResponse = await _dynamoDbClient.UpdateItemAsync(updateItemRequest);
            if (dynamoResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                // Handle the error appropriately
                ModelState.AddModelError("", "An error occurred while updating the movie. Please try again.");
                return View(model);
            }
           // Repopulate the model with the updated movie data
            model.Comments = movie.Comments;

            // Return the Comment view with the updated model
            return View("Comment", model);
            //return RedirectToAction("Comment", new { movieId = model.MovieId });
        }

     
    }
}