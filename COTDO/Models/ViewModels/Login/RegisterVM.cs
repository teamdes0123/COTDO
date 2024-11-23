using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COTDO.Models.ViewModels.Login
{
    public class RegisterVM
    {
        [Required]
        public string Cedula { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}