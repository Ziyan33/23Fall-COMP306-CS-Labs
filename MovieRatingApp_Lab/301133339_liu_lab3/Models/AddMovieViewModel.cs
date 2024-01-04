using System.ComponentModel.DataAnnotations;

namespace _301133339_liu_lab3.Models
{
    public class AddMovieViewModel
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public IFormFile? MovieFile { get; set; }

        [Required]
        public string? DirectorNames { get; set; }

        [Required]
        public string? Genre { get; set; }

        [Required]
        public DateTime? ReleaseTime { get; set; }
    }
}
