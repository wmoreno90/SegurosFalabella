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
        
        [Display(Name = "Compañía aseguradora")]
        public IEnumerable<SelectListItem> Producto { get; set; }

        [Required]
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

        [Required]
        [Display(Name = "Primer apellido")]
        public string PrimerApellido { get; set; }

        [Display(Name = "Segundo apellido")]
        public string SegundoApellido { get; set; }

        [Display(Name =("Tipo de documento"))]
        public IEnumerable<SelectListItem> TipoDocumento { get; set; }

        [Required]
        [Display(Name =("Tipo de documento"))]
        public string DocumentoId { get; set; }
        
        [Display(Name ="Tipo de documento")]
        public string NombreTipoDocumento { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        [Display(Name ="Teléfono")]
        public string Telefono { get; set; }

    }
}