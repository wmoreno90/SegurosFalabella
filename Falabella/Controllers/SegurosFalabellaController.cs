using Falabella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falabella.Controllers
{
    /// <summary>
    /// Controlador para el manejo de compañías aseguradoras, productos y clientes
    /// </summary>
    public class SegurosFalabellaController : Controller
    {
        /// <summary>
        /// Ventana de inicio
        /// </summary>
        /// <returns>Vista de inicio</returns>
        public ActionResult Inicio()
        {
            return View();
        }

        #region MANEJO DE COMPAÑÍAS ASEGURADORAS ALIADAS
        /// <summary>
        /// Método que retorna la vista de inserción de compañías aseguradoras aliadas
        /// </summary>
        /// <returns>Vista del ActionResult</returns>
        public ActionResult InsCompany()
        {
            return View();
        }

        /// <summary>
        /// Recibe el modelo de compañías aliadas diligenciado y lo inserta en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna a la vista de inserción de compañías alidas</returns>
        [HttpPost]
        public ActionResult InsCompany(AseguradorasViewModel model)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var aseguradora = new ASEGURADORAS
                {
                    NOMBRE = model.Nombre
                };

                db.ASEGURADORAS.Add(aseguradora);
                db.SaveChanges();

                return RedirectToAction("InsCompany");
            }
        }

        /// <summary>
        /// Obtiene todas las compañías aliadas existentes en la base de datos
        /// </summary>
        /// <returns>Vista con la lista de compañías aseguradoras aliadas</returns>
        public ActionResult GetCompanies()
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var aseguradorasDB = db.ASEGURADORAS.ToList();

                var aseguradoras = new List<AseguradorasViewModel>();

                foreach (var item in aseguradorasDB)
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
        }

        /// <summary>
        /// Elimina de la base de datos la compañía aliada seleccionada
        /// </summary>
        /// <param name="id">Id de la copañía aliada seleccionada</param>
        /// <returns>Retorna a la vista con la lista de compañías aliadas</returns>
        public ActionResult DeleteCompany(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                try
                {
                    var compania = db.ASEGURADORAS.FirstOrDefault(c => c.ASEGURADORA_ID == id);
                    db.ASEGURADORAS.Remove(compania);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                }                
            }

            return RedirectToAction("GetCompanies");
        }

        /// <summary>
        /// Edita la compañía aliada seleccionada
        /// </summary>
        /// <param name="id">Id de la compañía aliada seleccionada</param>
        /// <returns>Vista de edición de compañía aliada</returns>
        public ActionResult EditCompany(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var compania = db.ASEGURADORAS.FirstOrDefault(c => c.ASEGURADORA_ID == id);

                var aseguradora = new AseguradorasViewModel()
                {
                    Id = compania.ASEGURADORA_ID,
                    Nombre = compania.NOMBRE
                };

                return View(aseguradora);
            };
        }

        /// <summary>
        /// Recibe el modelo con los cambios hechos a la compañia aliada seleccionada y lo inserta en la base de datos
        /// </summary>
        /// <param name="model">Modelo con los datos de la compañia aliada que se editó</param>
        /// <returns>Vista con el listado de compañías aliadas</returns>
        [HttpPost]
        public ActionResult EditCompany(AseguradorasViewModel model)
        {
            using(var db = new SegurosFalabellaEntities())
            {
                var aseguradora = db.ASEGURADORAS.FirstOrDefault(a => a.ASEGURADORA_ID == model.Id);
                aseguradora.NOMBRE = model.Nombre;
                db.SaveChanges();
            }

            return RedirectToAction("GetCompanies");
        }
        #endregion

        #region MANEJO DE PRODUCTOS
        /// <summary>
        /// Inserta un producto en la base de datos
        /// </summary>
        /// <returns>Vista de inserción de producto</returns>
        public ActionResult InsProduct()
        {
            using(var db = new SegurosFalabellaEntities())
            {
                var model = new ProductosViewModel();

                var aseguradoras = db.ASEGURADORAS.ToList();              

                model.Aseguradora = GetSelectListItemAseguradoras(aseguradoras);

                return View(model);
            }            
        }

        /// <summary>
        /// Recibe el formulario con el modelo
        /// </summary>
        /// <param name="model">Modelo de productos</param>
        /// <returns>Retorna a la vista de inserción de productos</returns>
        [HttpPost]
        public ActionResult InsProduct(ProductosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                using (var db = new SegurosFalabellaEntities())
                {
                    var aseguradoras = db.ASEGURADORAS.ToList();

                    model.Aseguradora = GetSelectListItemAseguradoras(aseguradoras);

                    return View(model);
                }
            }

            using (var db = new SegurosFalabellaEntities())
            {
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
        }

        /// <summary>
        /// Obtiene la lista de productos de la base de datos
        /// </summary>
        /// <returns>Vista con el listado de productos</returns>
        public ActionResult GetProducts()
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var productos = db.PRODUCTOS.ToList();
                var aseguradoras = db.ASEGURADORAS.ToList();

                var listaProductos = new List<ProductosViewModel>();

                foreach (var item in productos)
                {
                    var producto = new ProductosViewModel()
                    {
                        Id = item.PRODUCTO_ID,
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
        }

        /// <summary>
        /// Elimina un producto de la base de datos
        /// </summary>
        /// <param name="id">Id del producto seleccionado</param>
        /// <returns>Vista con el listado de productos</returns>
        public ActionResult DeleteProduct(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                try
                {
                    var producto = db.PRODUCTOS.FirstOrDefault(c => c.PRODUCTO_ID == id);
                    db.PRODUCTOS.Remove(producto);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {

                }                
            }

            return RedirectToAction("GetProducts");
        }

        /// <summary>
        /// Vista de edición de un producto
        /// </summary>
        /// <param name="id">Id del producto a editar</param>
        /// <returns>Vista de edición del producto seleccionado</returns>
        public ActionResult EditProduct(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var producto = db.PRODUCTOS.FirstOrDefault(c => c.PRODUCTO_ID == id);
                var aseguradoras = db.ASEGURADORAS.ToList();
                
                var product = new ProductosViewModel()
                {
                    AseguradoraId = producto.ASEGURADORA_ID,
                    Producto = producto.PRODUCTO,
                    Prima = producto.PRIMA.ToString(),
                    Cobertura = producto.COBERTURA,
                    Asistencias = producto.ASISTENCIAS,
                    Aseguradora = GetSelectListItemAseguradoras(aseguradoras),
            };

                return View(product);
            };
        }

        /// <summary>
        /// Recibe el modelo con el producto editado y lo inserta en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProduct(ProductosViewModel model)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var producto = db.PRODUCTOS.FirstOrDefault(a => a.PRODUCTO_ID == model.Id);
                producto.PRODUCTO = model.Producto;
                producto.ASEGURADORA_ID = Convert.ToInt16(model.AseguradoraId);
                producto.PRIMA = Convert.ToInt32(model.Prima);
                producto.COBERTURA = model.Cobertura;
                producto.ASISTENCIAS = model.Asistencias;
                db.SaveChanges();
            }

            return RedirectToAction("GetProducts");
        }
        #endregion

        #region MANEJO DE CLIENTES
        /// <summary>
        /// Retorna la vista de inserción de clientes en la base de datos
        /// </summary>
        /// <returns>Vista de inserción de clientes</returns>
        public ActionResult InsClient()
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var model = new ClientesViewModel();

                var tiposDocumentos = db.DOCUMENTOS.ToList();
                var productos = db.PRODUCTOS.ToList();               

                model.TipoDocumento = GetSelectListItemDocumentos(tiposDocumentos);
                model.Producto = GetSelectListItemProductos(productos);

                return View(model);
            }            
        }

        /// <summary>
        /// Recibe el modelo con los datos del cliente y lo inserta en la base de datos
        /// </summary>
        /// <param name="model">Modelo con los datos del cliente</param>
        /// <returns>Vista de inserción de clientes</returns>
        [HttpPost]
        public ActionResult InsClient(ClientesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                using (var db = new SegurosFalabellaEntities())
                {
                    var tiposDocumentos = db.DOCUMENTOS.ToList();
                    var productos = db.PRODUCTOS.ToList();

                    model.TipoDocumento = GetSelectListItemDocumentos(tiposDocumentos);
                    model.Producto = GetSelectListItemProductos(productos);

                    return View(model);
                }
            }

            using (var db = new SegurosFalabellaEntities())
            {
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
        }

        /// <summary>
        /// Obtiene el listado de clientes
        /// </summary>
        /// <returns>Vista con el listado de clientes</returns>
        public ActionResult GetClients()
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var productos = db.PRODUCTOS.ToList();
                var aseguradoras = db.ASEGURADORAS.ToList();
                var documentos = db.DOCUMENTOS.ToList();
                var clientes = db.CLIENTES.ToList();

                var listaClientes = new List<ClientesViewModel>();

                foreach (var item in clientes)
                {
                    var cliente = new ClientesViewModel()
                    {
                        Id = item.CLIENTE_ID,
                        PrimerNombre = item.PRIMER_NOMBRE,
                        SegundoNombre = item.SEGUNDO_NOMBRE,
                        PrimerApellido = item.PRIMER_APELLIDO,
                        SegundoApellido = item.SEGUNDO_APELLIDO,
                        NombreTipoDocumento = GetTipoDocumento(item.TIPO_DOCUMENTO, documentos),
                        Documento = item.DOCUMENTO,
                        Telefono = item.TELEFONO,
                        NombreProducto = GetNombreProducto(item.PRODUCTO_ID, productos),
                        NombreAseguradora = GetNombreAseguradora(item.ASEGURADORA_ID, aseguradoras)
                    };

                    listaClientes.Add(cliente);
                }
                return View(listaClientes);
            }
        }

        /// <summary>
        /// Elimina un cliente de la base de datos
        /// </summary>
        /// <param name="id">Id del cliente a eliminar</param>
        /// <returns>Vista con el listado de clientes</returns>
        public ActionResult DeleteClient(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                try
                {
                    var cliente = db.CLIENTES.FirstOrDefault(c => c.CLIENTE_ID == id);
                    db.CLIENTES.Remove(cliente);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                }
            }                

            return RedirectToAction("GetClients");
        }

        /// <summary>
        /// Vista de edición de un cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditClient(int id)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var cliente = db.CLIENTES.FirstOrDefault(c => c.CLIENTE_ID == id);
                var aseguradoras = db.ASEGURADORAS.ToList();
                var productos = db.PRODUCTOS.ToList();
                var documentos = db.DOCUMENTOS.ToList();

                var client = new ClientesViewModel()
                {
                    PrimerNombre = cliente.PRIMER_NOMBRE,
                    SegundoNombre = cliente.SEGUNDO_NOMBRE,
                    PrimerApellido = cliente.PRIMER_APELLIDO,
                    SegundoApellido = cliente.SEGUNDO_APELLIDO,
                    TipoDocumento = GetSelectListItemDocumentos(documentos),
                    DocumentoId = cliente.TIPO_DOCUMENTO.ToString(),
                    Documento = cliente.DOCUMENTO,
                    Telefono = cliente.TELEFONO,
                    Producto = GetSelectListItemProductos(productos),
                    ProductoId = cliente.PRODUCTO_ID.ToString()
                };

                return View(client);
            };
        }

        /// <summary>
        /// Recibe los datos editados del cliente y los inserta en la base de datos
        /// </summary>
        /// <param name="model">Modelo con los datos del cliente</param>
        /// <returns>Vista con el listado de clientes</returns>
        [HttpPost]
        public ActionResult EditClient(ClientesViewModel model)
        {
            using (var db = new SegurosFalabellaEntities())
            {
                var productos = db.PRODUCTOS.ToList();
                var cliente = db.CLIENTES.FirstOrDefault(c => c.CLIENTE_ID == model.Id);
                cliente.PRIMER_NOMBRE = model.PrimerNombre;
                cliente.SEGUNDO_NOMBRE = model.SegundoNombre;
                cliente.PRIMER_APELLIDO = model.PrimerApellido;
                cliente.SEGUNDO_APELLIDO = model.SegundoApellido;
                cliente.TIPO_DOCUMENTO = Convert.ToInt16(model.DocumentoId);
                cliente.DOCUMENTO = model.Documento;
                cliente.TELEFONO = model.Telefono;
                cliente.PRODUCTO_ID = Convert.ToInt16(model.ProductoId);
                cliente.ASEGURADORA_ID = GetIdAseguradora(Convert.ToInt32(model.ProductoId), productos);

                db.SaveChanges();
            }

            return RedirectToAction("GetClients");
        }
        #endregion

        #region UTILS
        /// <summary>
        /// Convierte la lista de un modelo a una lista de tipo List<SelectListItem>
        /// </summary>
        /// <param name="aseguradoras">Lista del modelo de compañías aseguradoras</param>
        /// <returns>Lista de tipo List<SelectListItem></returns>
        private List<SelectListItem> GetSelectListItemAseguradoras(List<ASEGURADORAS> aseguradoras)
        {
            var listaAseguradoras = new List<SelectListItem>();

            foreach (var item in aseguradoras)
            {
                var aseguradora = new SelectListItem()
                {
                    Text = item.NOMBRE,
                    Value = item.ASEGURADORA_ID.ToString()
                };

                listaAseguradoras.Add(aseguradora);
            }

            return listaAseguradoras;
        }

        /// <summary>
        /// Convierte la lista de un modelo a una lista de tipo List<SelectListItem>
        /// </summary>
        /// <param name="tiposDocumentos">Lista del modelo de documentos</param>
        /// <returns>Lista de tipo List<SelectListItem></returns>
        private List<SelectListItem> GetSelectListItemDocumentos(List<DOCUMENTOS> tiposDocumentos)
        {
            var listaTiposDocumentos = new List<SelectListItem>();

            foreach (var item in tiposDocumentos)
            {
                var documento = new SelectListItem()
                {
                    Text = item.NOMBRE,
                    Value = item.DOCUMENTO_ID.ToString()
                };

                listaTiposDocumentos.Add(documento);
            }

            return listaTiposDocumentos;
        }

        /// <summary>
        /// Convierte la lista de un modelo a una lista de tipo List<SelectListItem>
        /// </summary>
        /// <param name="productos">Lista del modelo de productos</param>
        /// <returns>Lista de tipo List<SelectListItem></returns>
        private List<SelectListItem> GetSelectListItemProductos(List<PRODUCTOS> productos)
        {
            var ListaProductos = new List<SelectListItem>();

            foreach (var item in productos)
            {
                var producto = new SelectListItem()
                {
                    Text = item.PRODUCTO,
                    Value = item.PRODUCTO_ID.ToString()
                };

                ListaProductos.Add(producto);
            }

            return ListaProductos;
        }

        /// <summary>
        /// Obtiene el nombre de la compañía aseguradora a partir del listado de aseguradoras y 
        /// el Id de la aseguradora seleccionada
        /// </summary>
        /// <param name="id">Id de la aseguradora seleccionada</param>
        /// <param name="aseguradoras">Listado de aseguradoras</param>
        /// <returns>Nombre de la compañía aseguradora</returns>
        private string GetNombreAseguradora(int id, List<ASEGURADORAS> aseguradoras)
        {
            foreach (var item in aseguradoras)
            {
                if (item.ASEGURADORA_ID == id)
                {
                    return item.NOMBRE;
                }
            }

            return string.Format("Nombre de aseguradora desconocido");
        }

        /// <summary>
        /// Obtiene el nombre del producto a partir de la lista de productos y el Id del producto
        /// </summary>
        /// <param name="id">Id del producto</param>
        /// <param name="productos">Lista de productos</param>
        /// <returns>Nombre del producto</returns>
        private string GetNombreProducto(int id, List<PRODUCTOS> productos)
        {
            foreach (var item in productos)
            {
                if (item.PRODUCTO_ID == id)
                {
                    return item.PRODUCTO;
                }
            }

            return string.Format("Producto no identificado");
        }

        /// <summary>
        /// Obtiene el tipo de documento a partir de la lista de documentos y el Id del documento
        /// </summary>
        /// <param name="id">Id del documento</param>
        /// <param name="documentos">Lista de documentos</param>
        /// <returns>Nombre del tipo de documento</returns>
        private string GetTipoDocumento(int id, List<DOCUMENTOS> documentos)
        {
            foreach (var item in documentos)
            {
                if (item.DOCUMENTO_ID == id)
                {
                    return item.NOMBRE;
                }
            }

            return string.Format("Tipo de documento no identificado");
        }

        /// <summary>
        /// Obtiene el Id de la aseguradora a partir de la lista de productos y el Id del producto seleccionado
        /// </summary>
        /// <param name="id">Id del producto seleccionado</param>
        /// <param name="productos">Listado de productos</param>
        /// <returns>Id de la compañia aseguradora</returns>
        private int GetIdAseguradora(int id, List<PRODUCTOS> productos)
        {
            int aseguradoraId = 0;

            foreach (var item in productos)
            {
                if (item.PRODUCTO_ID == id)
                {
                    aseguradoraId = item.ASEGURADORA_ID;
                    break;
                }
            }

            return aseguradoraId;
        }
        #endregion
    }
}