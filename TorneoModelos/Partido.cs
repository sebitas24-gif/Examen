using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Partido
    {
        [Key] public int Id { get; set; }

        public string Grupo { get; set; } = string.Empty; // "Jornada 1", "Final"
        public DateTime FechaPartido { get; set; }

        // RELACIÓN CON TORNEO
        public int TorneoId { get; set; }
        public Torneo? Torneo { get; set; }

        // RELACIÓN CON EQUIPO (Local)
        public int EquipoLocalId { get; set; }
        public Equipo? EquipoLocal { get; set; }

        // Navegación 1:N a RegistroEstadistica
        public ICollection<RegistroEstadistica>? Estadisticas { get; set; }
    }
}
