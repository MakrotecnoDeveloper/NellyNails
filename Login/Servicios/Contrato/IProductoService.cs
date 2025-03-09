﻿using Plataforma.Models;
namespace Plataforma.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<bool> AgregarProductoAsync(string id_empresa, string codigo, string descripcion, float valor_neto, float valor_unitario, decimal stock, string categorias);
        List<Producto> ObtenerProductos();
        List<CategoriaProductos> ObtenerCategoriaProductos(int IdServicio);
        List<Producto> BuscarProductos(string searchTerm, string categoriaTerm);
        List<Producto> SinStock(string searchTerm, string categoriaTerm);
        List<Producto> BuscarProSinStock(string searchTerm, string categoriaTerm);
        Task<bool> AgregarStockAsync(string idProducto, int cantidad);
        IEnumerable<Producto> EditarStock(string id, int cantidad, int opcion);
        void EditarProducto(string codigo, string nombreProducto, float valorNeto, float valorVenta, int valorUnidad, int cantidad, string categoria, string idEmpresa, int estado);
        void EliminarProducto(string id);
        void InserPlataformaService(int idPlataforma, string descripcion, int valorventa, int valorneto, DateTime fechaInipago, DateTime fechaFinpago, int cantidad, string correo, string contrasena, int cedula, int estado);
        List<Plataformas> TraerPlataformasExistentes();
        List<Plataformasuscripcion> SuscripcionesActivas();
        Task<List<Plataformasuscripcion>> ObtenerSuscripcionesActivas(int plataformaId);
        Task<List<ClientePlataformaDTO>> ObtenerDatosSuscripcion(int suscripcionId);
        Task<List<ClientePlataformaDTO>> ObtenerDatosPlataforma(int suscripcionId);
        Task<bool> EliminarClienteAsync(int idClientePlataforma);
        void ServicioInsertarVentClientPlataforma(string nombrecliente, string celularcliente, string correo, string contrasena, int idPltfSuscripcion, int cantidad, string ppm, DateTime feciniplat, DateTime fecfinplat, int valorventa, int valorneto, int cedula, int estado, string clave);
        Task ActualizarCliente(int id, int estado, int idCliente);
        List<Producto> TraerProductosXCategoria(string categoria);
        ProveedorProductosViewModel TraerProveedorProductos(int cedula);
        void HistoricoCompra(int codfact, string cod_producto, decimal stock, int vneto, decimal vtotal, DateTime fechaIngreso, string tpventa, int idpdv);
        List<Factura> ObtenerFacturasPorFechaYUsuario(DateTime fecha, int cedula);
        DetallesFacturaViewModel ObtenerDetallesFactura(int codFactura);
    }
}
