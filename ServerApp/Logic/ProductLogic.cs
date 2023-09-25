using ServerApp.Database;
using ServerApp.Domain;

namespace ServerApp.Logic
{
    public class ProductLogic
	{
		private readonly SingletonDB _database;

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
			if (_database.buscarProductoPorNombre(nombre).Count == 0) {
                throw new Exception("No existe tal producto");
            } 
			else {
                return _database.buscarProductoPorNombre(nombre);
            }

		}

    }
}

