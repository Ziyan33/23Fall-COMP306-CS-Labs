using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookResearcher.Domain.Models
{
    public class LibraryBranch
    {
        [Key]
        public int LibraryID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ContactNumber { get; set; }

        public LibraryBranch()
        {
            BookAvailability = new HashSet<BookAvailability>();
        }
        // Navigation property
        public virtual ICollection<BookAvailability> BookAvailability { get; set; }
    }
}
