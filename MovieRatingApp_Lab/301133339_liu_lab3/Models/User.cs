namespace _301133339_liu_lab3.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? Salt { get; set; }
        // ... Other properties
        public virtual ICollection<Movie>? Movies { get; set; } // List of movies associated with the user

    }
}
