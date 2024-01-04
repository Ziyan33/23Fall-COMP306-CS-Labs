using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookResearcher.Domain.DTOs
{
    public class BookAvailabilityDTO
    {
        public int BookID { get; set; }
        public int LibraryID { get; set; }
        public int Quantity { get; set; }
        // Optional: Include related data as needed
        public string? BookTitle { get; set; }
        public string? LibraryBranchName { get; set; }
    }
}
