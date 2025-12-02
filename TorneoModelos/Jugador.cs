using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Jugador
    {
        [Key]
        public int Id { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;
        public int NumCamiseta { get; set; }

        // RELACIÓN CON EQUIPO
        public int EquipoId { get; set; }

        // [JsonIgnore]
        public Equipo? Equipo { get; set; }

    } 
}
