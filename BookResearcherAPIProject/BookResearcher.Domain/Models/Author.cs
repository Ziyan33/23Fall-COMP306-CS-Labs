using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookResearcher.Domain.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string? Name { get; set; }

        // Navigation property
        public virtual ICollection<BookAuthors>? BookAuthors { get; set; }
    }
}
