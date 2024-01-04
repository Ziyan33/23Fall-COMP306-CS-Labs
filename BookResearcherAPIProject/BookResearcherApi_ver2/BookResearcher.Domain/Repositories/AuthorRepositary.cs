using BookResearcher.Domain.Data;
using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookResearcher.Domain.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly BookResearcherContext _context;

        public AuthorRepository(BookResearcherContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues.Length != 1)
                throw new ArgumentException("GetByIdAsync for Author requires a single key value.", nameof(keyValues));

            var id = (int)keyValues[0];
            return await _context.Authors.FindAsync(id);
        }


        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Author>> FindAsync(Expression<Func<Author, bool>> predicate)
        {
            return await _context.Authors.Where(predicate).ToListAsync();
        }
    }

}
