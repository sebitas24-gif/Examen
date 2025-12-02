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
        [Key] public int Id { get; set; }

        public int TipoEstadistica { get; set; } // Ej: 1=Gol, 2=Amarilla, 3=Roja
        public int Cantidad { get; set; }

        // RELACIÓN CON JUGADOR
        public int JugadorId { get; set; }
        public Jugador? Jugador { get; set; }

        // RELACIÓN CON PARTIDO
        public int PartidoId { get; set; }
        public Partido? Partido { get; set; }
    }

}

