using System;
using Communication;
using ServerApp.Domain;
namespace ServerApp.Database
{
    public class SingletonDB
    {
        static readonly SettingsManager settingsMngr = new();
        private SingletonDB()
        {
            _productos = new List<Producto>();
            _usuarios = new List<Usuario>();
        }

        private static SingletonDB? _instance;
        private List<Producto> _productos;
        //private List<Calificacion> _calificaciones; // a implementar en otro PR
        private List<Usuario> _usuarios;


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
        public List<Producto> darListaProductos() { 
            return this._productos;
        }
      
        public List<Producto> darListaProductosCompradosPorUsuario(Usuario u) {
            List<Producto> retorno = new List<Producto>();
            foreach (Producto p in u.comprados) { 
                retorno.Add(p);
            }
            return retorno;
        }
      
        public List<Producto> agregarProducto(Producto producto)
        {
            _productos.Add(producto);
            return _productos;
        }
        public List<Producto> buscarProductoPorNombre(string nombre)
{
            List<Producto> retorno = new List<Producto>();
            foreach (Producto p in _productos) {
                if (p.Nombre.Contains(nombre)) { 
                    retorno.Add(p);
                }
            }
            return retorno;                
        }
        public Producto buscarUnProducto(string nombre) {
            foreach (Producto p in _productos)
            {
                if (p.Nombre.ToLower().Equals(nombre.ToLower()))
                {
                    return p;
                }
            }
            throw new Exception("No existe tal producto :(");
        }
        public Producto eliminarProducto(Producto p) {
            Producto ret = null;
            foreach (Producto prod in _productos.ToList()) {
                if (prod.Equals(p)) {
                    _productos.Remove(prod);
                    ret = prod;
                }
            }

            return ret;
        }
        public Producto modificarProducto (Producto p, string nombreOriginalProd) {

            Producto prodOriginal = buscarProductoPorNombre(nombreOriginalProd)[0];

            prodOriginal.Nombre = p.Nombre;
            prodOriginal.Descripcion = p.Descripcion;
            prodOriginal.Precio = p.Precio;
            prodOriginal.Imagen = p.Imagen;
            prodOriginal.Stock = p.Stock;

            return prodOriginal;
        }

        public bool existeProducto(Producto producto)
        {
            bool existe = false;

            foreach (Producto prod in _productos)
            {
                // Tomando como supuesto que no se permiten productos con el mismo nombre para validar los repetidos
                if (prod.Nombre == producto.Nombre)
                {
                    existe = true;
                }
            }
            return existe;
        }
        public bool tieneStock(Producto p) {
            if (p.Stock > 0)
            {
                return true;
            }
            else { 
                return false;
            }
        }

        public bool existeImagen(string nombreImagen)
        {
            bool existe = false;

            foreach (Producto prod in _productos)
            {
                // Tomando como supuesto que no se permiten imagenes con el mismo nombre en el servidor
                if (prod.Imagen  == nombreImagen)
                {
                    existe = true;
                }
            }
            return existe;
        }

        public List<Usuario> usuarios()
        {
            return _usuarios;
        }
        public List<Producto> agregarProductoACompras(Producto p,Usuario u) {
            u.comprados.Add(p);
            p.Stock--;
            return u.comprados;
        }

        //metodo para agregar usuarios
        public List<Usuario> agregarUsuario(Usuario usuario)
        {
            _usuarios.Add(usuario);
            return _usuarios;
        }

        public bool existeUsuario(Usuario usuario)
        {
            bool existe = false;

            foreach (Usuario user in _usuarios)
            {
                // Tomando como supuesto que no se permiten productos con el mismo nombre para validar los repetidos
                if (user.mail == usuario.mail)
                {
                    existe = true;
                }
            }
            return existe;
        }

        public Usuario buscarUsuario(string username)
        {
            return _usuarios.FirstOrDefault(u => u.mail == username);
        }
    }
}

