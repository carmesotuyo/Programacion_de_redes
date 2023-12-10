namespace ComprasServer.Database
{
    public class SingletonDB
    {
        private SingletonDB()
        {
            _compras = new List<Compra>();
        }

        private static SingletonDB? _instance;
        private List<Compra> _compras;


        private static readonly object _lock = new object();

        public static SingletonDB GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonDB();
                    }
                }
            }
            return _instance;
        }
        public List<Compra> darListaCompras()
        {
            return this._compras;
        }

        public List<Compra> agregarCompra(Compra compra)
        {
            _compras.Add(compra);
            return _compras;
        }

        public List<Compra> FiltrarCompras(
            string? usuario = null,
            string? nombreProducto = null,
            DateTime? fecha = null,
            float? precio = null)
        {
            var comprasFiltradas = _compras
                .Where(compra =>
                    (usuario == null || compra.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase)) &&
                    (nombreProducto == null || compra.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)) &&
                    (!fecha.HasValue || compra.Fecha.Date == fecha.Value.Date) &&
                    (!precio.HasValue || compra.Precio == precio.Value))
                .ToList();

            return comprasFiltradas;
        }

        public List<Compra> BuscarComprasPorProducto(string nombreProducto)
        {
            return _compras.Where(c => c.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Compra> BuscarComprasPorUsuario(string usuario)
        {
            return _compras.Where(c => c.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Compra> BuscarComprasPorFecha(DateTime fecha)
        {
            return _compras.Where(c => c.Fecha.Date == fecha.Date).ToList();
        }

        public List<Compra> BuscarComprasPorPrecio(float precio)
        {
            return _compras.Where(c => c.Precio == precio).ToList();
        }
    }
}
