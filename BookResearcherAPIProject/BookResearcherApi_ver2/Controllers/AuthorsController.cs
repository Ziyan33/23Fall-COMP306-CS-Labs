using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookResearcherApi_ver2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        // GET: api/authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDTO>(author));
        }

        // POST: api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            // Ensure AuthorID is not set or is zero for new authors
            author.AuthorID = 0;

            await _authorRepository.AddAsync(author);

            var createdAuthorDto = _mapper.Map<AuthorDTO>(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorID }, createdAuthorDto);
        }


        // PUT: api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDTO authorDto)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            // Map the updated fields from authorDto to author
            _mapper.Map(authorDto, author);

            // Explicitly set the AuthorID to ensure it matches the route
            author.AuthorID = id;

            await _authorRepository.UpdateAsync(author);

            return NoContent();
        }



        // DELETE: api/authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorRepository.DeleteAsync(author);
            return NoContent();
        }
        // PATCH: api/authors/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAuthor(int id, [FromBody] JsonPatchDocument<AuthorPatchDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var authorEntity = await _authorRepository.GetByIdAsync(id);
            if (authorEntity == null)
            {
                return NotFound();
            }

            var authorToPatch = _mapper.Map<AuthorPatchDTO>(authorEntity);
            patchDoc.ApplyTo(authorToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(authorToPatch, authorEntity);
            await _authorRepository.UpdateAsync(authorEntity);

            return NoContent();
        }


    }
}
