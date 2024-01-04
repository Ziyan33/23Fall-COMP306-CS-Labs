using System.ComponentModel.DataAnnotations;

namespace _301133339_liu_lab3.Models
{
    public class Comment
    {/*
        [Key]
        public int CommentId { get; set; }*/
        public string? Username { get; set; }
        public string? Text { get; set; }
        public int? Rating { get; set; }
    }
}
