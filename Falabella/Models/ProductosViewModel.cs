using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falabella.Models
{
    public class ProductosViewModel
    {
        public int Id { get; set; }
        
        [Display(Name ="Compañía aseguradora")]
        public IEnumerable<SelectListItem> Aseguradora { get; set; }

        [Display(Name ="Aseguradora")]
        public int AseguradoraId { get; set; }

        [Display(Name ="Aseguradora")]
        public string NombreAseguradora { get; set; }

        [Required]
        [Display(Name ="Nombre del producto")]
        public string Producto { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public string Prima { get; set; }

        [Required]
        public string Cobertura { get; set; }

        [Required]
        public string Asistencias { get; set; }
    }
}