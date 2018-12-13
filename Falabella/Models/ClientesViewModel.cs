using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falabella.Models
{
    public class ClientesViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Compañía aseguradora")]
        public IEnumerable<SelectListItem> Producto { get; set; }

        [Display(Name ="Producto")]
        public string ProductoId { get; set; }

        [Display(Name = "Aseguradora")]
        public string NombreAseguradora { get; set; }

        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Required]
        [Display(Name ="Primer nombre")]
        public string PrimerNombre { get; set; }

        [Display(Name = "Segundo nombre")]
        public string SegundoNombre { get; set; }

        [Display(Name = "Primer apellido")]
        public string PrimerApellido { get; set; }

        [Display(Name = "Segundo apellido")]
        public string SegundoApellido { get; set; }

        [Display(Name =("Tipo de documento"))]
        public IEnumerable<SelectListItem> TipoDocumento { get; set; }

        public string DocumentoId { get; set; }
        
        public string Documento { get; set; }

        [Display(Name ="Teléfono")]
        public string Telefono { get; set; }

    }
}