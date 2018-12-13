using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Falabella.Models
{
    public class AseguradorasViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Compañía aseguradora aliada")]
        public string Nombre { get; set; }
    }
}