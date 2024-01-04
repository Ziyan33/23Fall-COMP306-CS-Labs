using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace BookResearcher.Domain.Repositories
{
    public class BookAuthorsRepository : IRepository<BookAuthors>
    {
        private readonly BookResearcherContext _context;

        public BookAuthorsRepository(BookResearcherContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookAuthors>> GetAllAsync()
        {
            return await _context.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }

        // This method now conforms to the modified interface
        public async Task<BookAuthors?> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues.Length != 2)
                throw new ArgumentException("BookAuthors requires two key values for BookID and AuthorID.", nameof(keyValues));

            var bookId = (int)keyValues[0];
            var authorId = (int)keyValues[1];
            return await _context.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .FirstOrDefaultAsync(ba => ba.BookID == bookId && ba.AuthorID == authorId);
        }

        public async Task AddAsync(BookAuthors bookAuthors)
        {
            await _context.BookAuthors.AddAsync(bookAuthors);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookAuthors bookAuthors)
        {
            _context.BookAuthors.Update(bookAuthors);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BookAuthors bookAuthors)
        {
            _context.BookAuthors.Remove(bookAuthors);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookAuthors>> FindAsync(Expression<Func<BookAuthors, bool>> predicate)
        {
            return await _context.BookAuthors
                .Where(predicate)
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
    }
}
