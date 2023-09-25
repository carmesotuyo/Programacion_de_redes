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

        public List<Producto> agregarProducto(Producto producto)
        {
            _productos.Add(producto);
            Console.WriteLine("Productos: " + _productos[0].Nombre);
            return _productos;
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

        public bool existeImagen(string nombreImagen)
        {
            bool existe = false;

            foreach (Producto prod in _productos)
            {
                // Tomando como supuesto que no se permiten imagenes con el mismo nombre en el servidor
                if (prod.Imagen == nombreImagen)
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

        //metodo para agregar usuarios
        public List<Usuario> agregarUsuario(Usuario usuario)
        {
            _usuarios.Add(usuario);
            return _usuarios;
        }

        
    }
}

