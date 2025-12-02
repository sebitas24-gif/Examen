using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class RegistroEstadistica
    {
        [Key]
        public int Id { get; set; }

        // Ej: 1 = Gol, 2 = Amarilla, 3 = Roja
        public int TipoEstadistica { get; set; }
        public int Cantidad { get; set; }

        // RELACIÓN CON JUGADOR
        public int JugadorId { get; set; } // La llave (FK)
        // Falta esto para que se conecte:
        public Jugador? Jugador { get; set; }

        // RELACIÓN CON PARTIDO
        public int PartidoId { get; set; } // La llave (FK)
        // Falta esto para que se conecte:
        public Partido? Partido { get; set; }
    }

}

