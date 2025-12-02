using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Jugador
    {
        [Key] public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int NumCamiseta { get; set; }

        // RELACIÓN CON EQUIPO (FK)
        public int EquipoId { get; set; }

        // Navegacion
     
        public Equipo? Equipo { get; set; }

    } 
}
