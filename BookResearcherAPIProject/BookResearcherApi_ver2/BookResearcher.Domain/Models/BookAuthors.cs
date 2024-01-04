using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookResearcher.Domain.Models
{
    public class BookAuthors
    {
        [Key, Column(Order = 0)]
        public int BookID { get; set; }

        [Key, Column(Order = 1)]
        public int AuthorID { get; set; }

        // Navigation properties
        public virtual Book? Book { get; set; }
        public virtual Author? Author { get; set; }
    }
}
