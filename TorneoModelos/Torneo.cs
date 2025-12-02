using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class Torneo
    {
        [Key] public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        //Navegacion
        public List<Equipo>? Equipos { get; set; }
        public List<Partido>? Partidos { get; set; }
    }
}
