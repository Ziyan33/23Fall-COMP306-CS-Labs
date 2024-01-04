using Amazon.DynamoDBv2.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _301133339_liu_lab3.Models
{
    public class Movie
    {
        public string? movieId { get; set; } // DynamoDB primary key
        public string? Title { get; set; }
        public string? GenreId { get; set; }
        public string? DirectorNames { get; set; }
        public DateTime ReleaseTime { get; set; }
        public string? Genre { get; set; }
        public string? S3Key { get; set; }

        //----------------------------------------------
        //User
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        [NotMapped]
        public List<Comment>? Comments { get; set; } 
        public double? averageRating { get; set; }
        // ... Other metadata
    }
  /*  public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? Username { get; set; }
        public string? Text { get; set; }
        public int? Rating { get; set; }
    }*/
}
