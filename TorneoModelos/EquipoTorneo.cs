using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class EquipoTorneo
    {
        [Key]
        public int Id { get; set; }
        //FK
        public int TorneoId { get; set; }
        public int EquipoId { get; set; }

        //Navegacion
        public Torneo? Torneo { get; set; }
        public Equipo? Equipo { get; set; }
    }
}
