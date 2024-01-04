using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookResearcherApi_ver2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly IRepository<BookAuthors> _bookAuthorsRepository;
        private readonly IMapper _mapper;

        public BookAuthorsController(IRepository<BookAuthors> bookAuthorsRepository, IMapper mapper)
        {
            _bookAuthorsRepository = bookAuthorsRepository;
            _mapper = mapper;
        }

        // GET: api/bookauthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthorsDTO>>> GetAllBookAuthors()
        {
            var bookAuthors = await _bookAuthorsRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BookAuthorsDTO>>(bookAuthors));
        }

        // GET: api/bookauthors/{bookId}/{authorId}
        [HttpGet("{bookId}/{authorId}")]
        public async Task<ActionResult<BookAuthorsDTO>> GetBookAuthor(int bookId, int authorId)
        {
            var bookAuthor = await _bookAuthorsRepository.GetByIdAsync(new object[] { bookId, authorId });

            if (bookAuthor == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookAuthorsDTO>(bookAuthor));
        }

        // POST: api/bookauthors
        [HttpPost]
        public async Task<ActionResult<BookAuthorsDTO>> CreateBookAuthor([FromBody] BookAuthorsDTO bookAuthorsDto)
        {
            var bookAuthor = _mapper.Map<BookAuthors>(bookAuthorsDto);
            await _bookAuthorsRepository.AddAsync(bookAuthor);

            // Assuming you have a method to get the DTO by id, you might need to update this part
            return CreatedAtAction(nameof(GetBookAuthor), new { bookId = bookAuthor.BookID, authorId = bookAuthor.AuthorID }, bookAuthorsDto);
        }

        // This endpoint might not be necessary if you don't support updating the relationship itself
        // Updating the relationship might be more about adding or removing the association rather than changing it

        // DELETE: api/bookauthors/{bookId}/{authorId}
        [HttpDelete("{bookId}/{authorId}")]
        public async Task<IActionResult> DeleteBookAuthor(int bookId, int authorId)
        {
            var bookAuthor = await _bookAuthorsRepository.GetByIdAsync(new object[] { bookId, authorId });
            if (bookAuthor == null)
            {
                return NotFound();
            }

            await _bookAuthorsRepository.DeleteAsync(bookAuthor);
            return NoContent();
        }
    }
}
