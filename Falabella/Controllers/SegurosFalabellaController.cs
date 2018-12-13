using Falabella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falabella.Controllers
{
    public class SegurosFalabellaController : Controller
    {
        public ActionResult Inicio()
        {
            return View();
        }

        #region MANEJO DE COMPAÑÍAS ASEGURADORAS ALIADAS
        public ActionResult InsCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsCompany(AseguradorasViewModel model)
        {
            var db = new SegurosFalabellaEntities();

            var aseguradora = new ASEGURADORAS
            {
                NOMBRE = model.Nombre
            };

            db.ASEGURADORAS.Add(aseguradora);
            db.SaveChanges();

            return RedirectToAction("InsCompany");
        }

        public ActionResult GetCompanies()
        {
            var db = new SegurosFalabellaEntities();

            var aseguradorasDB = db.ASEGURADORAS.ToList();

            var aseguradoras = new List<AseguradorasViewModel>();

            foreach(var item in aseguradorasDB)
            {
                var aseguradora = new AseguradorasViewModel()
                {
                    Id = item.ASEGURADORA_ID,
                    Nombre = item.NOMBRE
                };

                aseguradoras.Add(aseguradora);
            }

            return View(aseguradoras);
        }
        #endregion

        #region MANEJO DE PRODUCTOS
        public ActionResult InsProduct()
        {
            var model = new ProductosViewModel();
            var listaAseguradoras = new List<SelectListItem>();
            var db = new SegurosFalabellaEntities();

            var aseguradoras = db.ASEGURADORAS.ToList();

            foreach(var item in aseguradoras)
            {
                var aseguradora = new SelectListItem()
                {
                    Text = item.NOMBRE,
                    Value = item.ASEGURADORA_ID.ToString()
                };

                listaAseguradoras.Add(aseguradora);
            }
            
            model.Aseguradora = listaAseguradoras;

            return View(model);
        }

        [HttpPost]
        public ActionResult InsProduct(ProductosViewModel model)
        {
            var db = new SegurosFalabellaEntities();

            var producto = new PRODUCTOS()
            {
                ASEGURADORA_ID = Convert.ToInt32(model.AseguradoraId),
                PRODUCTO = model.Producto,
                PRIMA = Convert.ToInt32(model.Prima),
                COBERTURA = model.Cobertura,
                ASISTENCIAS = model.Asistencias
            };

            db.PRODUCTOS.Add(producto);
            db.SaveChanges();

            return RedirectToAction("InsProduct");
        }

        public ActionResult GetProducts()
        {
            var db = new SegurosFalabellaEntities();

            var productos = db.PRODUCTOS.ToList();
            var aseguradoras = db.ASEGURADORAS.ToList();

            var listaProductos = new List<ProductosViewModel>();

            foreach(var item in productos)
            {
                var producto = new ProductosViewModel()
                {
                    NombreAseguradora = GetNombreAseguradora(item.ASEGURADORA_ID, aseguradoras),
                    Producto = item.PRODUCTO,
                    Prima = item.PRIMA.ToString(),
                    Cobertura = item.COBERTURA,
                    Asistencias = item.ASISTENCIAS
                };

                listaProductos.Add(producto);
            }

            return View(listaProductos);
        }

        private string GetNombreAseguradora(int id, List<ASEGURADORAS> aseguradoras)
        {
            foreach(var item in aseguradoras)
            {
                if(item.ASEGURADORA_ID == id)
                {
                    return item.NOMBRE;
                }
            }

            return string.Format("Nombre de aseguradora desconocido");
        }
        #endregion

        #region MANEJO DE CLIENTES
        public ActionResult InsClient()
        {
            var model = new ClientesViewModel();
            var listaDocumentos = new List<SelectListItem>();
            var ListaProductos = new List<SelectListItem>();
            var db = new SegurosFalabellaEntities();

            var tiposDocumentos = db.DOCUMENTOS.ToList();

            var productos = db.PRODUCTOS.ToList();

            foreach (var item in tiposDocumentos)
            {
                var documento = new SelectListItem()
                {
                    Text = item.NOMBRE,
                    Value = item.DOCUMENTO_ID.ToString()
                };

                listaDocumentos.Add(documento);
            }

            foreach (var item in productos)
            {
                var documento = new SelectListItem()
                {
                    Text = item.PRODUCTO,
                    Value = item.PRODUCTO_ID.ToString()
                };

                ListaProductos.Add(documento);
            }

            model.TipoDocumento = listaDocumentos;
            model.Producto = ListaProductos;

            return View(model);
        }

        [HttpPost]
        public ActionResult InsClient(ClientesViewModel model)
        {            
            var db = new SegurosFalabellaEntities();
            var productos = db.PRODUCTOS.ToList();

            var cliente = new CLIENTES()
            {
                PRODUCTO_ID = Convert.ToInt32(model.ProductoId),
                ASEGURADORA_ID = GetIdAseguradora(Convert.ToInt32(model.ProductoId), productos),
                PRIMER_NOMBRE = model.PrimerNombre,
                SEGUNDO_NOMBRE = model.SegundoNombre,
                PRIMER_APELLIDO = model.PrimerApellido,
                SEGUNDO_APELLIDO = model.SegundoApellido, 
                TIPO_DOCUMENTO = Convert.ToInt16(model.DocumentoId),
                DOCUMENTO = model.Documento,
                TELEFONO = model.Telefono
            };

            db.CLIENTES.Add(cliente);
            db.SaveChanges();

            return RedirectToAction("InsClient");
        }

        private int GetIdAseguradora(int id, List<PRODUCTOS> productos)
        {
            foreach(var item in productos)
            {
                if(item.PRODUCTO_ID == id)
                {
                    return item.ASEGURADORA_ID;
                }
            }

            return 0;
        }
        #endregion
    }
}