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
    public class LibraryBranchesController : ControllerBase
    {
        private readonly IRepository<LibraryBranch> _libraryBranchRepository;
        private readonly IMapper _mapper;

        public LibraryBranchesController(IRepository<LibraryBranch> libraryBranchRepository, IMapper mapper)
        {
            _libraryBranchRepository = libraryBranchRepository;
            _mapper = mapper;
        }

        // GET: api/librarybranches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryBranchDTO>>> GetAllLibraryBranches()
        {
            var libraryBranches = await _libraryBranchRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<LibraryBranchDTO>>(libraryBranches));
        }

        // GET: api/librarybranches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryBranchDTO>> GetLibraryBranch(int id)
        {
            var libraryBranch = await _libraryBranchRepository.GetByIdAsync(id);

            if (libraryBranch == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LibraryBranchDTO>(libraryBranch));
        }
        [HttpPost]
        public async Task<ActionResult<LibraryBranchDTO>> CreateLibraryBranch([FromBody] LibraryBranchDTO libraryBranchDto)
        {
            var libraryBranch = _mapper.Map<LibraryBranch>(libraryBranchDto); // No LibraryID should be set here
            await _libraryBranchRepository.AddAsync(libraryBranch);

            // Map back to DTO to include the generated LibraryID
            var createdLibraryBranchDto = _mapper.Map<LibraryBranchDTO>(libraryBranch);
            return CreatedAtAction(nameof(GetLibraryBranch), new { id = libraryBranch.LibraryID }, createdLibraryBranchDto);
        }


        // PUT: api/librarybranches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLibraryBranch(int id, [FromBody] LibraryBranchDTO libraryBranchDto)
        {
            if (id != libraryBranchDto.LibraryID)
            {
                return BadRequest();
            }

            var libraryBranch = await _libraryBranchRepository.GetByIdAsync(id);
            if (libraryBranch == null)
            {
                return NotFound();
            }

            _mapper.Map(libraryBranchDto, libraryBranch);
            await _libraryBranchRepository.UpdateAsync(libraryBranch);

            return NoContent();
        }

        // DELETE: api/librarybranches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibraryBranch(int id)
        {
            var libraryBranch = await _libraryBranchRepository.GetByIdAsync(id);
            if (libraryBranch == null)
            {
                return NotFound();
            }

            await _libraryBranchRepository.DeleteAsync(libraryBranch);
            return NoContent();
        }
    }
}
