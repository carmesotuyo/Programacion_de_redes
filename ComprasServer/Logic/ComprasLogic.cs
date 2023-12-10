using ComprasServer.Database;

namespace ComprasServer.Logic
{
    public class ComprasLogic
    {
        private readonly SingletonDB _database;

        public ComprasLogic()
        {
            _database = SingletonDB.GetInstance();
        }

        public Compra agregarCompra(Compra compra)
        {
            _database.agregarCompra(compra);
            return compra;
        }

        public List<Compra> darListadoCompras(
            string? usuario = null,
            string? nombreProducto = null,
            DateTime? fecha = null,
            float? precio = null)
        {
            // Llama al método centralizado en SingletonDB para filtrar compras
            return _database.FiltrarCompras(usuario, nombreProducto, fecha, precio);
        }

        public List<Compra> BuscarComprasPorProducto(string nombreProducto)
        {
            return _database.BuscarComprasPorProducto(nombreProducto);
        }

        public List<Compra> BuscarComprasPorUsuario(string usuario)
        {
            return _database.BuscarComprasPorUsuario(usuario);
        }

        public List<Compra> BuscarComprasPorFecha(DateTime fecha)
        {
            return _database.BuscarComprasPorFecha(fecha);
        }

        public List<Compra> BuscarComprasPorPrecio(float precio)
        {
            return _database.BuscarComprasPorPrecio(precio);
        }
    }
}
