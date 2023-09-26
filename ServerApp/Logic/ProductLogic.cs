using ServerApp.Database;
using ServerApp.Domain;
using Communication;

namespace ServerApp.Logic
{
    public class ProductLogic
	{
		private readonly SingletonDB _database;
		private readonly UserLogic _userLogic;

		public ProductLogic()
		{
			_database = SingletonDB.GetInstance();
			_userLogic = new UserLogic();
		}

		public Producto publicarProducto(Producto producto, string username)
		{
			validarProductoRepetido(producto);
			Usuario usuario = _userLogic.buscarUsuario(username);
			_database.agregarProducto(producto);
			usuario.agregarProductoAPublicados(producto);
			return _database.buscarProductoPorNombre(producto.Nombre)[0]; // confirmamos que se guardó
		}

		private void validarProductoRepetido(Producto producto)
		{
            ValidarNombreRepetido(producto.Nombre);
			if(producto.Imagen != Protocol.NoImage) ValidarImagenRepetida(producto.Imagen);
            if (_database.existeProducto(producto))
			{
				throw new Exception("El producto ya existe publicado en el Marketplace, te ganaron de mano :(");
			}
		}

        public void ValidarImagenRepetida(string imagen)
        {
            if (_database.existeImagen(imagen)) throw new Exception("Una imagen con ese nombre ya existe, probá cambiándolo :)");
        }

        public void ValidarNombreRepetido(string nombre)
        {
            if (buscarProductoPorNombre(nombre).Count > 0) throw new Exception("El nombre de producto ya existe :(");
        }

        public List<Producto> BuscarProductos(string palabra)
		{
			existeProducto(palabra);
			return buscarProductoPorNombre(palabra);
		}


        private List<Producto> buscarProductoPorNombre(string nombre) {
            return _database.buscarProductoPorNombre(nombre);
        }

		public Producto eliminarProducto(string nombreProd, string username)
		{
			existeProducto(nombreProd);
			Producto p = buscarProductoPorNombre(nombreProd)[0];
			tienePermisos(username, p);

            _database.eliminarProducto(p);
            return p;

        }

		public Producto modificarProducto (Producto producto, string nombreOriginal, string user)
		{
            existeProducto(nombreOriginal);
            Producto prodAModificar = buscarProductoPorNombre(nombreOriginal)[0];
			tienePermisos(user, prodAModificar);
			validarProductoRepetido(producto);
			_database.modificarProducto(producto, nombreOriginal);
			return _database.buscarProductoPorNombre(producto.Nombre)[0]; // para validar que el objeto en bd realmente se actualizó
		}

		// Método para validar que el usuario puede actualizar la imagen de su producto
		// Devuelve el nombre de la imagen anterior para eliminarla
		public string CambiarImagen(Producto producto, string user)
		{
			existeProducto(producto.Nombre);
            Producto prodAModificar = buscarProductoPorNombre(producto.Nombre)[0];
            tienePermisos(user, prodAModificar);
			ValidarImagenRepetida(producto.Imagen);
			return prodAModificar.Imagen;
        }


        public Producto VerMasProducto(string nombreProd)
		{
			existeProducto(nombreProd);
			return buscarProductoPorNombre(nombreProd)[0];
        }

        private void existeProducto(string nombre)
        {
            if (buscarProductoPorNombre(nombre).Count == 0) throw new Exception("El producto ingresado no existe :(");
        }

        private void tienePermisos(string usuario, Producto producto)
        {
            if (!esQuienPublicoElProducto(usuario, producto)) throw new Exception("No tiene permiso para modificar un producto que no publicó");
        }

        private bool esQuienPublicoElProducto(string username, Producto prod)
        {
            Usuario user = _userLogic.buscarUsuario(username);
            bool es = false;
            if (user.publicados.Contains(prod)) es = true;
            return es;
        }
    }
}

