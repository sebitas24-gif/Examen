using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoModelos;

namespace Examen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidosController : ControllerBase
    {
        private readonly ExamenContext _context;

        public PartidosController(ExamenContext context)
        {
            _context = context;
        }

        // GET: api/Partidoss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partido>>> GetPartido()
        {
            return await _context.Partidoss.ToListAsync();
        }

        // GET: api/Partidoss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partido>> GetPartido(int id)
        {
            var partido = await _context.Partidoss.FindAsync(id);

            if (partido == null)
            {
                return NotFound();
            }

            return partido;
        }

        // PUT: api/Partidoss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartido(int id, Partido partido)
        {
            if (id != partido.Id)
            {
                return BadRequest();
            }

            _context.Entry(partido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Partidoss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Partido>> PostPartido(Partido partido)
        {
            _context.Partidoss.Add(partido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartido", new { id = partido.Id }, partido);
        }

        // DELETE: api/Partidoss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var partido = await _context.Partidoss.FindAsync(id);
            if (partido == null)
            {
                return NotFound();
            }

            _context.Partidoss.Remove(partido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartidoExists(int id)
        {
            return _context.Partidoss.Any(e => e.Id == id);
        }
    }
}
