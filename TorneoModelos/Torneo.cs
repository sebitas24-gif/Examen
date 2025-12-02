using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Torneo
    {
        [Key] public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Liga, Copa, Mixto
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Navegación M:M a Equipo a través de EquipoTorneo
        public ICollection<EquipoTorneo> EquiposTorneos { get; set; } = new List<EquipoTorneo>();

        // Navegación 1:N a Partido
        public ICollection<Partido>? Partidos { get; set; }
    }
}
