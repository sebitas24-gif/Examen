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
        [Key] public int Id { get; set; }

        public string Grupo { get; set; } = string.Empty; // "Jornada 1", "Final"
        public DateTime FechaPartido { get; set; } // DateTime es vital para ordenar

        // RELACIÓN CON TORNEO (Saber a cuál pertenece)
        public int TorneoId { get; set; }
        public Torneo? Torneo { get; set; }

        // RELACIÓN CON EQUIPO (Local)
        // Al llamarlo "EquipoLocalId", EF podría confundirse, así que aquí
        // sí es mejor ser explícito con el nombre, pero mantenlo simple.
        public int EquipoLocalId { get; set; }
        public Equipo? EquipoLocal { get; set; }

        // Si quisieras agregar visitante en el futuro, solo copias lo de arriba
        // y le pones EquipoVisitanteId / EquipoVisitante
    }
}
