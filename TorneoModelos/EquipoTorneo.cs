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
        public int TorneoId { get; set; }

        // Clave Foránea a Equipo (Parte de la PK compuesta)
        public int EquipoId { get; set; }

        // Navegacion
        public Torneo Torneo { get; set; } = null!;
        public Equipo Equipo { get; set; } = null!;
    }
}
