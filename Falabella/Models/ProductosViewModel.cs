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

        [Required]
        [Display(Name ="Compañía aseguradora")]
        public IEnumerable<SelectListItem> Aseguradora { get; set; }

        [Display(Name ="Aseguradora")]
        public string AseguradoraId { get; set; }

        [Display(Name ="Aseguradora")]
        public string NombreAseguradora { get; set; }

        [Display(Name ="Nombre del producto")]
        public string Producto { get; set; }

        public string Prima { get; set; }

        public string Cobertura { get; set; }

        public string Asistencias { get; set; }
    }
}