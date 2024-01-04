namespace _301133339_liu_lab3.Models
{
    public class CommentViewModel
    {

        /*   public string? MovieId { get; set; }
           public string? MovieTitle { get; set; } // To display in the Movie Information Box
           public string? DirectorNames { get; set; } // To display in the Movie Information Box
           public string? Genre { get; set; } // To display in the Movie Information Box
           public DateTime ReleaseTime { get; set; } // To display in the Movie Information Box
           public int? Rating { get; set; } // To capture the star rating
           public string? CommentText { get; set; } // To capture the user's comment
           public List<Comment> Comments { get; set; } = new List<Comment>(); // To display the list of comments
   */
        public string? MovieId { get; set; }
        public string? Title { get; set; }
        public string? DirectorNames { get; set; }
        public string? Genre { get; set; }
        public DateTime ReleaseTime { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public Comment NewComment { get; set; } = new Comment();
    }
}
