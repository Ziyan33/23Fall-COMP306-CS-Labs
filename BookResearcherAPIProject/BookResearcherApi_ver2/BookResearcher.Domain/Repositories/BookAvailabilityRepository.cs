using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace BookResearcher.Domain.Repositories
{
    public class BookAvailabilityRepository : IRepository<BookAvailability>
    {
        private readonly BookResearcherContext _context;

        public BookAvailabilityRepository(BookResearcherContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookAvailability>> GetAllAsync()
        {
            return await _context.BookAvailability
                .Include(ba => ba.Book)
                .Include(ba => ba.LibraryBranch)
                .ToListAsync();
        }

        public async Task<BookAvailability?> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues.Length != 2)
                throw new ArgumentException("GetByIdAsync for BookAvailability requires two key values for BookID and LibraryID.", nameof(keyValues));

            var bookId = (int)keyValues[0];
            var libraryId = (int)keyValues[1];
            return await _context.BookAvailability
                .Include(ba => ba.Book)
                .Include(ba => ba.LibraryBranch)
                .FirstOrDefaultAsync(ba => ba.BookID == bookId && ba.LibraryID == libraryId);
        }

        public async Task AddAsync(BookAvailability bookAvailability)
        {
            await _context.BookAvailability.AddAsync(bookAvailability);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookAvailability bookAvailability)
        {
            _context.BookAvailability.Update(bookAvailability);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BookAvailability bookAvailability)
        {
            _context.BookAvailability.Remove(bookAvailability);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookAvailability>> FindAsync(Expression<Func<BookAvailability, bool>> predicate)
        {
            return await _context.BookAvailability
                .Where(predicate)
                .Include(ba => ba.Book)
                .Include(ba => ba.LibraryBranch)
                .ToListAsync();
        }
    }
}
