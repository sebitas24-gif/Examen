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
    public class RegistroEstadisticasController : ControllerBase
    {
        private readonly ExamenContext _context;

        public RegistroEstadisticasController(ExamenContext context)
        {
            _context = context;
        }

        // GET: api/RegistroEstadisticas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroEstadistica>>> GetRegistroEstadistica()
        {
            return await _context.RegistroEstadisticas.ToListAsync();
        }

        // GET: api/RegistroEstadisticas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroEstadistica>> GetRegistroEstadistica(int id)
        {
            var registroEstadistica = await _context.RegistroEstadisticas.FindAsync(id);

            if (registroEstadistica == null)
            {
                return NotFound();
            }

            return registroEstadistica;
        }

        // PUT: api/RegistroEstadisticas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroEstadistica(int id, RegistroEstadistica registroEstadistica)
        {
            if (id != registroEstadistica.Id)
            {
                return BadRequest();
            }

            _context.Entry(registroEstadistica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroEstadisticaExists(id))
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

        // POST: api/RegistroEstadisticas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RegistroEstadistica>> PostRegistroEstadistica(RegistroEstadistica registroEstadistica)
        {
            _context.RegistroEstadisticas.Add(registroEstadistica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroEstadistica", new { id = registroEstadistica.Id }, registroEstadistica);
        }

        // DELETE: api/RegistroEstadisticas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroEstadistica(int id)
        {
            var registroEstadistica = await _context.RegistroEstadisticas.FindAsync(id);
            if (registroEstadistica == null)
            {
                return NotFound();
            }

            _context.RegistroEstadisticas.Remove(registroEstadistica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroEstadisticaExists(int id)
        {
            return _context.RegistroEstadisticas.Any(e => e.Id == id);
        }
    }
}
