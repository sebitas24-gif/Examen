using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoModelos; // Tu namespace de modelos

namespace Examen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorneosController : ControllerBase
    {
        private readonly ExamenContext _context;

        public TorneosController(ExamenContext context)
        {
            _context = context;
        }

        // =========================================================
        // MÉTODOS ESTÁNDAR (CRUD)
        // =========================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Torneo>>> GetTorneos()
        {
            // Incluimos Equipos y Partidos para ver todo el detalle
            return await _context.Torneos
                .Include(t => t.Equipos)
                .Include(t => t.Partidos)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Torneo>> GetTorneo(int id)
        {
            var torneo = await _context.Torneos
                .Include(t => t.Equipos)
                .Include(t => t.Partidos)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (torneo == null) return NotFound();
            return torneo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTorneo(int id, Torneo torneo)
        {
            if (id != torneo.Id) return BadRequest();
            _context.Entry(torneo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TorneoExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Torneo>> PostTorneo(Torneo torneo)
        {
            _context.Torneos.Add(torneo);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTorneo", new { id = torneo.Id }, torneo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTorneo(int id)
        {
            var torneo = await _context.Torneos.FindAsync(id);
            if (torneo == null) return NotFound();
            _context.Torneos.Remove(torneo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // =========================================================
        // MÉTODOS DE LÓGICA DE NEGOCIO (GENERAR CALENDARIO)
        // =========================================================

        [HttpPost("{id}/iniciar")]
        public async Task<IActionResult> IniciarTorneo(int id)
        {
            var torneo = await _context.Torneos
                .Include(t => t.Equipos)
                .Include(t => t.Partidos)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (torneo == null) return NotFound("Torneo no encontrado");

            // Validar cantidad mínima de equipos (ej: 4 para pruebas, 16 para real)
            if (torneo.Equipos == null || torneo.Equipos.Count < 4)
                return BadRequest($"Faltan equipos. Actuales: {torneo.Equipos?.Count ?? 0}");

            // Limpiar partidos anteriores si se reinicia
            if (torneo.Partidos != null && torneo.Partidos.Any())
                _context.Partidos.RemoveRange(torneo.Partidos);

            // Generar lógica según el tipo
            if (torneo.Tipo == "Mixto" || torneo.Tipo == "Liga")
            {
                GenerarFaseGrupos(torneo);
            }
            else
            {
                return BadRequest("Tipo de torneo no soportado para generación automática aún.");
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Torneo iniciado y calendario generado." });
        }

        // --- LÓGICA PRIVADA PARA GENERAR PARTIDOS ---

        private void GenerarFaseGrupos(Torneo torneo)
        {
            // Mezclar equipos al azar
            var rng = new Random();
            var equipos = torneo.Equipos.OrderBy(x => rng.Next()).ToList();

            // Asignar grupos (A, B, C, D)
            string[] letrasGrupos = { "A", "B", "C", "D" };

            // Si hay pocos equipos, usamos menos grupos
            int numGrupos = equipos.Count >= 8 ? 4 : 1;
            int tamanoGrupo = equipos.Count / numGrupos;

            for (int i = 0; i < numGrupos; i++)
            {
                var letraActual = letrasGrupos[i];
                // Tomar los equipos que tocan en este grupo
                var equiposDelGrupo = equipos.Skip(i * tamanoGrupo).Take(tamanoGrupo).ToList();

                // Actualizar su propiedad Grupo en la BD
                foreach (var eq in equiposDelGrupo)
                {
                    eq.Grupo = letraActual;
                    _context.Entry(eq).State = EntityState.Modified; // Marcar como modificado
                }

                // Generar "Todos contra todos" dentro del grupo
                for (int j = 0; j < equiposDelGrupo.Count; j++)
                {
                    for (int k = j + 1; k < equiposDelGrupo.Count; k++)
                    {
                        var partido = new Partido
                        {
                            TorneoId = torneo.Id,
                            EquipoLocalId = equiposDelGrupo[j].Id,
                            EquipoVisitanteId = equiposDelGrupo[k].Id,
                            FechaPartido = DateTime.UtcNow.AddDays(1), // Mañana
                            Grupo = "Fase de Grupos",
                            // Estado por defecto es "Pendiente" según tu modelo
                        };
                        _context.Partidos.Add(partido);
                    }
                }
            }
        }

        private bool TorneoExists(int id)
        {
            return _context.Torneos.Any(e => e.Id == id);
        }
    }
}
