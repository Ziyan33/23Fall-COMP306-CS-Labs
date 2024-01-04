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
    public class BookRepository : IRepository<Book>
    {
        private readonly BookResearcherContext _context;

        public BookRepository(BookResearcherContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookAvailability)
                    .ThenInclude(ba => ba.LibraryBranch)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues.Length != 1)
                throw new ArgumentException("GetByIdAsync for Book requires a single key value.", nameof(keyValues));

            var id = (int)keyValues[0];
            return await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookAvailability)
                    .ThenInclude(ba => ba.LibraryBranch)
                .FirstOrDefaultAsync(b => b.BookID == id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _context.Books
                .Where(predicate)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookAvailability)
                    .ThenInclude(ba => ba.LibraryBranch)
                .ToListAsync();
        }
    }
}
