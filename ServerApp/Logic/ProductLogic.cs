using ServerApp.Database;
using ServerApp.Domain;
using Communication;
using System;

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
		public List<Producto> darListadoProductos() { 
			return _database.darListaProductos();
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

		public string modificarProducto (Producto producto, string user, string atributo, string nuevoValor)
		{
            string mensajeACliente = "";

            existeProducto(producto.Nombre);
            tienePermisos(user, producto);
            switch (atributo)
            {
                case "nombre":
                    ValidarNombreRepetido(nuevoValor);
                    producto.Nombre = nuevoValor;
                    mensajeACliente = "Nombre del producto actualizado con éxito.";
                    break;

                case "descripcion":
                    producto.Descripcion = nuevoValor;
                    mensajeACliente = "Descripción del producto actualizada con éxito.";
                    break;

                case "precio":
                    if (float.TryParse(nuevoValor, out float nuevoPrecio))
                    {
                        producto.Precio = nuevoPrecio;
                        mensajeACliente = "Precio del producto actualizado con éxito.";
                    }
                    else
                    {
                        mensajeACliente = "El nuevo valor de precio no es válido.";
                    }
                    break;

                case "stock":
                    if (int.TryParse(nuevoValor, out int nuevoStock))
                    {
                        producto.Stock = nuevoStock;
                        mensajeACliente = "Stock del producto actualizado con éxito.";
                    }
                    else
                    {
                        mensajeACliente = "El nuevo valor de stock no es válido.";
                    }
                    break;

                default:
                    mensajeACliente = "Atributo no válido. No se realizó ninguna actualización.";
                    break;

            }

			return mensajeACliente;
		}
		// Método para validar que el usuario puede actualizar la imagen de su producto
		// Devuelve el nombre de la imagen anterior para eliminarla
		public string CambiarImagen(Producto producto, string user, string nuevaImagen)
		{
			existeProducto(producto.Nombre);
            Producto prodAModificar = buscarProductoPorNombre(producto.Nombre)[0];
			string imagenAnterior = prodAModificar.Imagen;
            tienePermisos(user, prodAModificar);
			ValidarImagenRepetida(nuevaImagen);
			producto.Imagen = nuevaImagen;
			return imagenAnterior;
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

