using HR_API.Data;
using HR_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        public readonly EntryDbContext _context;

        public EntryController(EntryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Entry>> Get()
        {
            return await _context.Entries.ToListAsync();
        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(Entry), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            return entry == null ? NotFound() : Ok(entry);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(Entry entry)
        {
            await _context.Entries.AddAsync(entry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.EmployeeID }, entry);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Entry entry)
        {
            if (id != entry.EmployeeID) return BadRequest();

            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
