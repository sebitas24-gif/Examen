using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneoModelos
{
    public class RegistroEstadistica
    {
        [Key] public int Id { get; set; }
        public int TipoEstadistica { get; set; }
        public int Cantidad { get; set; }
        public int JugadorId { get; set; }
        public int PartidoId { get; set; }

    }
}
