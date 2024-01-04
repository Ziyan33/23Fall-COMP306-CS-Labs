using System.ComponentModel.DataAnnotations;

namespace _301133339_liu_lab3.Models
{
    public class ModifyMovieViewModel
    {
        public string? MovieId { get; set; }
        public string? Title { get; set; }
       
        public IFormFile? MovieFile { get; set; }
        public string? DirectorNames { get; set; }
        public string? Genre { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public string? S3Key { get; set; }
    }

}
