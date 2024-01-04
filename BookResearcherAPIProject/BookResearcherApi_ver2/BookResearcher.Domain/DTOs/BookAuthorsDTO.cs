using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookResearcher.Domain.DTOs
{
    public class BookAuthorsDTO
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        // Optional: Include related data as needed
        public string? BookTitle { get; set; }
        public string? AuthorName { get; set; }
    }
}
