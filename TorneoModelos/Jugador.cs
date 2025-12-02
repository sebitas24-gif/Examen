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

        // CORRECCIÓN: Agregado { get; set; } para que se guarde en la BD
        public string NombreCompleto { get; set; } = string.Empty;

        public int NumCamiseta { get; set; }

        // Clave Foránea (FK)
        public int EquipoId { get; set; }

        //Navegacion 
        public Equipo? Equipo { get; set; }

    } 
}
