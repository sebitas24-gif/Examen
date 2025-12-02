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
        [Key]
        public int Id { get; set; }

        public string Grupo { get; set; } = string.Empty;
        public DateTime FechaPartido { get; set; }

        public int EquipoLocalId { get; set; }

        //Navegacion
        public Equipo? EquipoLocal { get; set; }
    }
}
