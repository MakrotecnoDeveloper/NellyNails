﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Models;
using Plataforma.Servicios.Contrato;

namespace Plataforma.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoservice;
        public ProductoController(IProductoService productoservice)
        {
            _productoservice = productoservice;
        }
        public IActionResult Index()
        {
            var productos = _productoservice.ObtenerProductos();
            return View(productos);
        }
        public IActionResult Insertar() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Insertar(string id_empresa, string codigo, string descripcion, float valorNeto, float valorVenta, decimal stock, string categoria)
        {

            if (ModelState.IsValid)
            {
                // Lógica para agregar el producto usando _productoService
                var resultado = await _productoservice.AgregarProductoAsync(id_empresa, codigo, descripcion, valorNeto, valorVenta, stock, categoria);

                if (resultado)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        [Authorize]
        [HttpGet]
        public IActionResult Buscar(string searchTerm, string categoriaTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) {
                searchTerm = "";
            }else
            {
                categoriaTerm = "";
            }
            var productosEncontrados = _productoservice.BuscarProductos(searchTerm, categoriaTerm);
            return PartialView("_TablaProductos", productosEncontrados);
        }
        [HttpGet]
        public IActionResult BuscarSinStock(string searchTerm, string categoriaTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            else
            {
                categoriaTerm = "";
            }
            var productosSinStock = _productoservice.SinStock(searchTerm, categoriaTerm);
            return PartialView("_TablaProductos", productosSinStock);
        }
        [HttpGet]
        public IActionResult BuscarProximosSinStock(string searchTerm, string categoriaTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            else
            {
                categoriaTerm = "";
            }
            var productosEncontrados = _productoservice.BuscarProSinStock(searchTerm, categoriaTerm);
            return PartialView("_TablaProductos", productosEncontrados);
        }
        public IActionResult Editar(string id)
        {
            string categoriaTerm = "";
            var editarProducto = _productoservice.BuscarProductos(id, categoriaTerm);
            foreach (var producto in editarProducto)
            {
                // Realiza acciones con cada producto, por ejemplo:
                Console.WriteLine($"ID: {producto.Cod_Producto}, Nombre: {producto.NombreProducto}");
            }
            if (editarProducto.Any())
            {
                // Oculta la tabla de productos y muestra la tabla temporal
                return View(editarProducto);
            }
            else
            {
                // Producto no encontrado, maneja la lógica adecuada
                Console.WriteLine("No hay productos con ese codigo referenciado");
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult EditarProducto(string codigo, string nombreProducto, float valorNeto, float valorVenta, int valorUnidad, int cantidad, string categoria, string idEmpresa, int estado)
        {
            // Llama al método EditarProducto del servicio de productos
            _productoservice.EditarProducto(codigo, nombreProducto, valorNeto, valorVenta, valorUnidad, cantidad, categoria, idEmpresa, estado);

            // Redirige a la acción que deseas después de editar el producto
            return RedirectToAction("Index"); // Por ejemplo, redirigir a la página de inicio del controlador de productos
        }
        public IActionResult Stock(string id, int cantidad, int opcion)
        {
            var productosActualizados = _productoservice.EditarStock(id, cantidad, opcion);
            if (productosActualizados.Any())
            {
                return View("Index", productosActualizados);
            }
            else
            {
                ViewBag.Mensaje = "Producto no encontrado";
                return View("_Mensaje");
            }
        }
        public IActionResult Eliminar(string id)
        {
            _productoservice.EliminarProducto(id);
            return RedirectToAction("Index");
        }
        public IActionResult VisualizarProducto(string id)
        {
            string categoriaTerm = "";
            string searchTerm = id;
            var traerProductos = _productoservice.BuscarProductos(searchTerm, categoriaTerm);
            return View(traerProductos);
        }
        /*Visualizacion de  Recargas de Plataformas */
        public IActionResult FormPlataforma()
        {
            var traerPlataformasExistentes = _productoservice.TraerPlataformasExistentes();
            return View(traerPlataformasExistentes);
        }
        [HttpPost]
        public IActionResult InsertPlataforma(int idPlataforma, string plataformas, string descripcion, int valorventa, int valorneto, DateTime fechaInipago, DateTime fechaFinpago, int cantidad, string correo, string contrasena, int cedula, int estado) 
        {
            if(string.IsNullOrEmpty(descripcion) || valorventa <= 0 || valorneto <= 0 || fechaInipago == DateTime.MinValue || fechaFinpago == DateTime.MinValue || cantidad <= 0 || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena) || cedula <= 0 || estado <= 0)
            {
                var mensaje = "Error: Hay campos sin informacion digitada, revisar todo lo llenado.";
                TempData["ErrorMessage"] = mensaje;
                return RedirectToAction("Error", "Errores");
            }else
            {
                _productoservice.InserPlataformaService(idPlataforma, descripcion, valorventa, valorneto, fechaInipago, fechaFinpago, cantidad, correo, contrasena, cedula, estado);
                return View("FormPlataforma");
            }
        }
        public IActionResult FormInserClienPlatf()
        {
            var traerPlataformas = _productoservice.SuscripcionesActivas();
            return View(traerPlataformas);
        }
        public IActionResult InsertVentClientPltf(string nombrecliente, string celularcliente, string correo, string contrasena, int idPltfSuscripcion, int cantidad, string ppm, DateTime feciniplat, DateTime fecfinplat, int valorventa, int valorneto, int cedula, int estado, string clave)
        {
            _productoservice.ServicioInsertarVentClientPlataforma(nombrecliente, celularcliente, correo, contrasena, idPltfSuscripcion, cantidad, ppm, feciniplat, fecfinplat, valorventa, valorneto, cedula, estado, clave);
            return RedirectToAction("formInserClienPlatf", "Producto");
        }
        public IActionResult FormVisuPlatf()
        {
            var searchPlataform = _productoservice.TraerPlataformasExistentes();
            return View(searchPlataform);
        }
        public IActionResult FormVisuCta()
        {
            var searchPlataform = _productoservice.TraerPlataformasExistentes();
            return View(searchPlataform);
        }
        [HttpGet]
        public async Task<IActionResult> GetSuscripcionesActivas(int plataformaId)
        {
                var suscripciones = await _productoservice.ObtenerSuscripcionesActivas(plataformaId);
                return Json(suscripciones);
        }

        // Obtener datos de clientes relacionados con una suscripción
        [HttpGet]
        public async Task<IActionResult> GetDatosSuscripcion(int suscripcionId)
        {
            var datos = await _productoservice.ObtenerDatosSuscripcion(suscripcionId);
            return Json(datos);
        }
        [HttpGet]
        public async Task<IActionResult> GetDatosPlataforma(int suscripcionId)
        {
            var datos = await _productoservice.ObtenerDatosPlataforma(suscripcionId);
            return Json(datos);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _productoservice.EliminarClienteAsync(id);
            if (resultado)
            {
                return Ok(new { message = "Cliente eliminado con éxito." });
            }
            return BadRequest(new { message = "Error al eliminar el cliente o cliente no encontrado." });
        }
        [HttpPost]
        public async Task<IActionResult> EditarEstadoCta(int id, int estado, int idCliente)
        {
            Console.WriteLine("IDCLIENTEPLATAFORMA: " + id + " ESTADO: " + estado + " IDCLIENTE " + idCliente);
                await _productoservice.ActualizarCliente(id, estado, idCliente);
                return Json(new { success = true });
        }
        //Visualizar productos existentes para vender en la pagina inicial
        [HttpGet]
        public IActionResult ProductosExistentesVenta(int IdServicio)
        {
            var traerCategoriasExistentes = _productoservice.ObtenerCategoriaProductos(IdServicio);
            return View("../Home/productosExistentesVenta", traerCategoriasExistentes);
        }
        public IActionResult TraerProductoXCategoria(string categoria)
        {
            var productosTraidos = _productoservice.TraerProductosXCategoria(categoria);
            return PartialView("../Home/_ProductosParciales", productosTraidos);
        }
        public IActionResult ComprasProductos()
        {
            var cedulaClaim = User.FindFirst("Cedula");
            if (cedulaClaim != null && int.TryParse(cedulaClaim.Value, out int cedula))
            {
                var proveedorProducto = _productoservice.TraerProveedorProductos(cedula);
                return View(proveedorProducto);
            }else
            {
                var mensaje = "El claim 'Cedula' no existe o la conversión falló.";
                TempData["ErrorMessage"] = mensaje;
                return RedirectToAction("Error", "Errores");
            }
                
        }
        [HttpPost]
        public IActionResult GuardarHistoricoCompra(List<ProductoViewModel> productos, int codfact, DateTime fechaRegistro, string tpventa)
        {
            int idPDv = 1;
            foreach (var producto in productos)
            {
                // Insertar pedido por cada producto
                _productoservice.HistoricoCompra(
                    codfact,
                    producto.Codigo,
                    producto.Stock,
                    producto.VNeto,
                    producto.VTotal,
                    fechaRegistro,
                    tpventa,
                    idPDv
                );
            }
            return RedirectToAction("ComprasProductos");
        }
        public IActionResult VisualizarCompras()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BuscarFactXFecha(DateTime fechaEscoger)
        {
            var cedulaClaim = User.FindFirst("Cedula");
            if (cedulaClaim != null && int.TryParse(cedulaClaim.Value, out int cedula))
            {
                var facturas = _productoservice.ObtenerFacturasPorFechaYUsuario(fechaEscoger, cedula);

                // Transformar las facturas a un objeto más ligero si es necesario
                var result = facturas.Select(f => new
                {
                    codFactura = f.Cod_factura,
                    fechaVenta = f.FechaVenta.ToShortDateString() // Formatear la fecha
                });

                return Json(result);
            }
            else
            {
                var mensaje = "El claim 'Cedula' no existe o la conversión falló.";
                TempData["ErrorMessage"] = mensaje;
                return RedirectToAction("Error", "Errores");
            }
                
        }
        [HttpGet]
        public IActionResult ObtenerDetallesFactura(int codFactura)
        {
            var detallesFactura = _productoservice.ObtenerDetallesFactura(codFactura); // Llama al servicio para obtener los detalles

            if (detallesFactura == null)
            {
                var mensaje = "Resultado Null, revisar datos.";
                TempData["ErrorMessage"] = mensaje;
                return RedirectToAction("Error", "Errores");
            }

            // Devuelve la vista parcial con los detalles de la factura
            return PartialView("_DetallesFactura", detallesFactura);
        }

    }
}
