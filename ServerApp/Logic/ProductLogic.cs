using ServerApp.Database;
using ServerApp.Domain;

namespace ServerApp.Logic
{
    public class ProductLogic
	{
		private readonly SingletonDB _database;
		private readonly UserLogic _userLogic;

		public ProductLogic()
		{
			_database = SingletonDB.GetInstance();
		}

		public Producto publicarProducto(Producto producto, Usuario usuario)
		{
			validarProductoRepetido(producto);
			_database.agregarProducto(producto);
			usuario.agregarProductoAPublicados(producto);
			return producto; // podria traer el producto de la db confirmando que se guardó
		}

		private void validarProductoRepetido(Producto producto)
		{
			if (_database.existeProducto(producto))
			{
				throw new Exception("El producto ya existe publicado en el Marketplace, te ganaron de mano :(");
			} else if (_database.existeImagen(producto.Imagen))
			{
				throw new Exception("Una imagen con ese nombre ya existe, probá cambiándolo :)");
			}
		}

		public List<Producto> buscarProductoPorNombre(string nombre) {
            return _database.buscarProductoPorNombre(nombre);
        }

		public Producto eliminarProducto(string nombre, string username) {
			Usuario user = _userLogic.buscarUsuario(username);
			
			if (buscarProductoPorNombre(nombre).Count > 0)
			{
				Producto p = buscarProductoPorNombre(nombre)[0];
                if (user.publicados.Contains(p))
                {
                    _database.eliminarProducto(p);
                    return p;
                } else
				{
					throw new Exception("El usuario no puede eliminar un producto que no publicó");
				}
			}
			else {
				throw new Exception("El nombre de producto ingresado no existe");
			}
 
		}
		public Producto modificarProducto (Producto producto, string nombre) {
			
			Producto prodAModificar = buscarProductoPorNombre(nombre)[0];
			//validarProductoRepetido(producto);
			_database.modificarProducto(producto, nombre);

			return producto;
		}
		public Producto eliminarProducto(string nombre) {
			Producto p = null;
			p = buscarProductoPorNombre(nombre)[0];
			_database.eliminarProducto(p);
			return p;
		}

		public bool esQuienPublicoElProducto(string username, Producto prod)
		{
            Usuario user = _userLogic.buscarUsuario(username);
			bool es = false;
			if (user.publicados.Contains(prod)) es = true;
			return es;
        }
    }
}

