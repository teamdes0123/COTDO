using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COTDO.Models
{
    public class AuditableBaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}