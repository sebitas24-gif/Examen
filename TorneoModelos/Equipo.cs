using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Equipo
    {
        [Key] 
        public int Id { get; set; }

        public string? Nombre { get; set; }
        public string? Grupo { get; set; }

        // FK
        public int TorneoId { get; set; }

        // Navegación 
        public Torneo? Torneo { get; set; }

        public List<Jugador>? Jugadores { get; set; }
    }
}
