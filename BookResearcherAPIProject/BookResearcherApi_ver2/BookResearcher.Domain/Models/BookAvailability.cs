using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookResearcher.Domain.Models
{
    public class BookAvailability
    {
        [Key, Column(Order = 0)]
        public int BookID { get; set; }

        [Key, Column(Order = 1)]
        public int LibraryID { get; set; }

        public int Quantity { get; set; }

        // Navigation properties
        public virtual Book? Book { get; set; }
        public virtual LibraryBranch? LibraryBranch { get; set; }
    }
}
