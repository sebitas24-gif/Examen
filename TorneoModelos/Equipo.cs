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
        [Key]  public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty; 

        // RELACIÓN CON TORNEO (FK Directa)
        // EF detecta automáticamente que TorneoId es la llave de Torneo
        public int TorneoId { get; set; }

        // Navegación (para acceder a los datos del torneo desde el equipo)
        public Torneo? Torneo { get; set; }

        // Lista de sus jugadores
        public List<Jugador>? Jugadores { get; set; }
    }
}
