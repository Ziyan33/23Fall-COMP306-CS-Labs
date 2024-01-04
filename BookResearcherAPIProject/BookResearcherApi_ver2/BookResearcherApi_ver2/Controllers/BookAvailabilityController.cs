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
    public class BookAvailabilityController : ControllerBase
    {
        private readonly IRepository<BookAvailability> _bookAvailabilityRepository;
        private readonly IMapper _mapper;

        public BookAvailabilityController(IRepository<BookAvailability> bookAvailabilityRepository, IMapper mapper)
        {
            _bookAvailabilityRepository = bookAvailabilityRepository;
            _mapper = mapper;
        }

        // GET: api/bookavailability
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAvailabilityDTO>>> GetAllBookAvailability()
        {
            var bookAvailabilities = await _bookAvailabilityRepository.GetAllAsync();
            var bookAvailabilityDTOs = _mapper.Map<IEnumerable<BookAvailabilityDTO>>(bookAvailabilities);
            return Ok(bookAvailabilityDTOs);
        }

        // GET: api/bookavailability/{bookId}/{libraryId}
        [HttpGet("{bookId}/{libraryId}")]
        public async Task<ActionResult<BookAvailabilityDTO>> GetBookAvailability(int bookId, int libraryId)
        {
            var bookAvailability = await _bookAvailabilityRepository.GetByIdAsync(new object[] { bookId, libraryId });

            if (bookAvailability == null)
            {
                return NotFound();
            }

            var bookAvailabilityDTO = _mapper.Map<BookAvailabilityDTO>(bookAvailability);
            return Ok(bookAvailabilityDTO);
        }

        // POST: api/bookavailability
        [HttpPost]
        public async Task<ActionResult<BookAvailabilityDTO>> CreateBookAvailability([FromBody] BookAvailabilityDTO bookAvailabilityDto)
        {
            var bookAvailability = _mapper.Map<BookAvailability>(bookAvailabilityDto);
            await _bookAvailabilityRepository.AddAsync(bookAvailability);

            return CreatedAtAction(nameof(GetBookAvailability),
                new { bookId = bookAvailability.BookID, libraryId = bookAvailability.LibraryID },
                bookAvailabilityDto);
        }

        // PUT: api/bookavailability/{bookId}/{libraryId}
        [HttpPut("{bookId}/{libraryId}")]
        public async Task<IActionResult> UpdateBookAvailability(int bookId, int libraryId, [FromBody] BookAvailabilityDTO bookAvailabilityDto)
        {
            if (bookId != bookAvailabilityDto.BookID || libraryId != bookAvailabilityDto.LibraryID)
            {
                return BadRequest();
            }

            var bookAvailability = await _bookAvailabilityRepository.GetByIdAsync(new object[] { bookId, libraryId });
            if (bookAvailability == null)
            {
                return NotFound();
            }

            _mapper.Map(bookAvailabilityDto, bookAvailability);
            await _bookAvailabilityRepository.UpdateAsync(bookAvailability);

            return NoContent();
        }

        // DELETE: api/bookavailability/{bookId}/{libraryId}
        [HttpDelete("{bookId}/{libraryId}")]
        public async Task<IActionResult> DeleteBookAvailability(int bookId, int libraryId)
        {
            var bookAvailability = await _bookAvailabilityRepository.GetByIdAsync(new object[] { bookId, libraryId });
            if (bookAvailability == null)
            {
                return NotFound();
            }

            await _bookAvailabilityRepository.DeleteAsync(bookAvailability);
            return NoContent();
        }
    }
}
