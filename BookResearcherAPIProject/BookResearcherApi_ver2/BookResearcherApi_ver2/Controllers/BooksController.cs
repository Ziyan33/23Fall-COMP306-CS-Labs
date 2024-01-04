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
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDTO>(book));
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] BookDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto); // BookID is not set here
            await _bookRepository.AddAsync(book);

            // Map back to DTO to include the generated BookID
            var createdBookDto = _mapper.Map<BookDTO>(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.BookID }, createdBookDto);
        }


        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            if (id != bookDto.BookID)
            {
                return BadRequest();
            }

            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _mapper.Map(bookDto, book);
            await _bookRepository.UpdateAsync(book);

            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteAsync(book);
            return NoContent();
        }
    }
}
