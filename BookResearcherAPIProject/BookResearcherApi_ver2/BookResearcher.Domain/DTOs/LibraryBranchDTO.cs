using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookResearcher.Domain.DTOs
{
    public class LibraryBranchDTO
    {
        public int LibraryID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ContactNumber { get; set; }
    }
}
