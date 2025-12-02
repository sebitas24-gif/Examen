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
        public string NombreTorneo { get; set; }
        public string TipoTorneo { get; set; } // "Liga", "Copa", "Mixto" 

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<Equipo>? Equipos { get; set; }
        public List<Partido>? Partidos { get; set; } 
    }
}
