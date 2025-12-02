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
        [Key] public int Id { get; set; }
        public string NombreCompleto = string.Empty;
        public int NumCamiseta { get; set; }
        public int EquipoId { get; set; } 


    } 
}
