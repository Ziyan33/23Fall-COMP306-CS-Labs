using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookResearcher.Domain.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }

        // Initialize the collection properties in the constructor
        public Book()
        {
            BookAuthors = new HashSet<BookAuthors>();
            BookAvailability = new HashSet<BookAvailability>();
        }
        // Navigation properties
        public virtual ICollection<BookAuthors> BookAuthors { get; set; }
        public virtual ICollection<BookAvailability> BookAvailability { get; set; }
    }
}
