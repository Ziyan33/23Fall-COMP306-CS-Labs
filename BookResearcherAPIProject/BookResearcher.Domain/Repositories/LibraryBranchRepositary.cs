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
    public class LibraryBranchRepository : IRepository<LibraryBranch>
    {
        private readonly BookResearcherContext _context;

        public LibraryBranchRepository(BookResearcherContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LibraryBranch>> GetAllAsync()
        {
            return await _context.LibraryBranches
                .Include(lb => lb.BookAvailability)
                    .ThenInclude(ba => ba.Book)
                .ToListAsync();
        }

        public async Task<LibraryBranch?> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues.Length != 1)
                throw new ArgumentException("GetByIdAsync for LibraryBranch requires a single key value.", nameof(keyValues));

            var id = (int)keyValues[0];
            return await _context.LibraryBranches
                .Include(lb => lb.BookAvailability)
                    .ThenInclude(ba => ba.Book)
                .FirstOrDefaultAsync(lb => lb.LibraryID == id);
        }

        public async Task AddAsync(LibraryBranch libraryBranch)
        {
            await _context.LibraryBranches.AddAsync(libraryBranch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LibraryBranch libraryBranch)
        {
            _context.LibraryBranches.Update(libraryBranch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LibraryBranch libraryBranch)
        {
            _context.LibraryBranches.Remove(libraryBranch);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LibraryBranch>> FindAsync(Expression<Func<LibraryBranch, bool>> predicate)
        {
            return await _context.LibraryBranches
                .Where(predicate)
                .Include(lb => lb.BookAvailability)
                    .ThenInclude(ba => ba.Book)
                .ToListAsync();
        }
    }
}
