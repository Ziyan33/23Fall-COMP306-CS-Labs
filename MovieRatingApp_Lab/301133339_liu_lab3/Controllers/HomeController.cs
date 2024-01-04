using _301133339_liu_lab3.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _301133339_liu_lab3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager; // Assuming ApplicationUser is your user entity
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _moviesTableName = "Movies"; // Ensure this matches your DynamoDB table name

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IAmazonDynamoDB dynamoDbClient)
        {
            _logger = logger;
            _userManager = userManager;
            _dynamoDbClient = dynamoDbClient;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var movies = await GetMoviesFromDynamoDB(user.Id);
                UserDashboardModel model = new UserDashboardModel
                {
                    Movies = movies
                };
                return View("UserDashboard", model);
            }
            else
            {
                // Handle the case when the user is not found
                return View("Error");
            }
        }

        public async Task<List<Movie>> GetMoviesFromDynamoDB(string userId)
        {
            var request = new ScanRequest
            {
                TableName = _moviesTableName,
                FilterExpression = "UserId = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_Id", new AttributeValue { S = userId }}
                }
            };

            var response = await _dynamoDbClient.ScanAsync(request);

            var movies = new List<Movie>();

            foreach (var item in response.Items)
            {
                movies.Add(new Movie
                {
                    // Assuming your Movie class has properties like Title, Genre, Rating, etc.
                    movieId = item["movieId"].S,
                    Title = item["Title"].S,
                    Genre = item["Genre"].S,
                    S3Key = item["S3Key"].S,
                    // ... Add other properties here ...
                });
            }

            return movies;
        }

        /*  public async Task<IActionResult> FilterByGenre(string genreSort)
          {
              List<Movie> filteredMovies = new List<Movie>();

              if (!string.IsNullOrEmpty(genreSort))
              {
                  // Query DynamoDB using the GenreIndex GSI
                  var queryRequest = new QueryRequest
                  {
                      TableName = _moviesTableName,
                      IndexName = "GenreIndex", // Ensure this matches the name of your GSI
                      KeyConditionExpression = "genre = :v_genre", // Ensure "genre" matches the partition key of your GSI
                      ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                  {":v_genre", new AttributeValue { S = genreSort }}
              }
                  };

                  try
                  {
                      var response = await _dynamoDbClient.QueryAsync(queryRequest);

                      // Map the response items to your Movie model
                      foreach (var item in response.Items)
                      {
                          var movie = new Movie
                          {
                              movieId = item["movieId"].S,
                              Title = item["title"].S, // Ensure this matches the attribute names projected by the index
                              Genre = genreSort,
                              ReleaseTime = DateTime.Parse(item["releaseTime"].S), // Ensure this matches the attribute names projected by the index
                              averageRating = double.Parse(item["averageRating"].N) // Ensure this matches the attribute names projected by the index
                                                                                    // ... other properties
                          };
                          filteredMovies.Add(movie);
                      }
                  }
                  catch (AmazonDynamoDBException ex)
                  {
                      _logger.LogError(ex, "Error occurred while querying DynamoDB.");
                      // Handle the exception appropriately
                  }
              }
              else
              {
                  // Handle the case where no genre is selected (maybe return all movies?)
                  filteredMovies = await GetMoviesFromDynamoDB(_userManager.GetUserId(User));
              }

              // Create a UserDashboardModel and populate it with the filtered movies
              var model = new UserDashboardModel
              {
                  Movies = filteredMovies
              };

              // Return the UserDashboard view with the filtered movies
              return View("UserDashboard", model);
          }*/

        public async Task<IActionResult> SortMovies(string ratingSort, string genreSort)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                // Handle the case when the user is not found
                return View("Error");
            }

            var movies = await GetMoviesFromDynamoDB(user.Id);

            // Filter by genre if one is selected and it is not "all"
            if (!string.IsNullOrEmpty(genreSort) && genreSort.ToLower() != "all")
            {
                movies = movies.Where(m => m.Genre.Equals(genreSort, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sort by rating
            if (!string.IsNullOrEmpty(ratingSort))
            {
                movies = ratingSort == "highest" ? movies.OrderByDescending(m => m.averageRating).ToList() : movies.OrderBy(m => m.averageRating).ToList();
            }

            UserDashboardModel model = new UserDashboardModel
            {
                Movies = movies
            };

            return View("UserDashboard", model);
        }


        // Ensure you have an Error action
        [Route("Home/Error")]
        public IActionResult Error()
        {
            // You can add logging here
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    }
}