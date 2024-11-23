using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COTDO.Models
{
    public class User : AuditableBaseEntity
    {
        public string Cedula {  get; set; }
        public string Nombres { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public int? CodCargo { get; set; }
        public int? TiempoEnServicio { get; set; }
        public string FechaIngreso { get; set; }
        public int? IdPeriodo { get; set; }
    }
}