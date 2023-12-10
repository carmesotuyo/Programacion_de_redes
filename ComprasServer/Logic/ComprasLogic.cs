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

        public List<Compra> darListadoCompras()
        {
            return _database.darListaCompras();
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
