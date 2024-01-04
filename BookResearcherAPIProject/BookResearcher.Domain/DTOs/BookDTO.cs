using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookResearcher.Domain.DTOs
{
    public class BookDTO
    {
        public int BookID { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
    }
}
