using System;
using System.Text;
using Communication;
using ServerApp.Domain;
using ServerApp.Logic;

namespace ServerApp.Controllers
{
	public class ProductController
	{
        private readonly ProductLogic _productLogic = new ProductLogic();
        private readonly UserLogic _userLogic = new UserLogic();
        private readonly string _filesPath;

		public ProductController(string filesPath)
        {
            _filesPath = filesPath;
        }

        public void agregarProductosBase(string nombre,string desc,float precio, int stock, string username) {
            Producto p = new Producto(nombre,desc,precio,stock);

            _productLogic.publicarProducto(p,username);
        }
        public string darProductos() {
            StringBuilder retorno = new StringBuilder();
            int i = 1;
            if (_productLogic.darListadoProductos().ToList().Count > 0)
            {
                foreach (Producto p in _productLogic.darListadoProductos().ToList())
                {
                    retorno.AppendLine(i + "- " + p.Nombre);
                    i++;
                }
            }
            else {
                return "No existen productos registrados";
            }

            return retorno.ToString();
            
        }

        public string modificarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileHandler)
        {
            string mensajeACliente = "";
            try
            {
                string info = msgHandler.ReceiveMessage();
                string[] datos = info.Split('#');
                string username = datos[0];
                string nombreProd = datos[1];
                string atributoAModificar = datos[2].ToLower();
                string nuevoValor = datos[3];

                Producto p = _productLogic.buscarUnProducto(nombreProd);

                if(atributoAModificar == "imagen")
                {
                    string imagenAnterior = _productLogic.CambiarImagen(p, username, DameNombreImagen(nuevoValor));
                    if (imagenAnterior != Protocol.NoImage) BorrarImagen(_filesPath, imagenAnterior);
                    fileHandler.ReceiveFile(_filesPath);
                    mensajeACliente = "Imagen del producto actualizada con éxito.";
                } else
                {
                    mensajeACliente = _productLogic.modificarProducto(p, username, atributoAModificar, nuevoValor);
                }
            }
            catch(Exception e)
            {
                mensajeACliente = e.Message;
            }
            
            return mensajeACliente;

        }

		public string publicarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileCommsHandler)
        {
            string mensajeAlCliente = "";
            try
			{
                // Capturamos la informacion
                string info = msgHandler.ReceiveMessage();
                string[] datos = info.Split("#");
                string user = datos[0];
                string nombre = datos[1];
                string descripcion = datos[2];
                float precio = float.Parse(datos[3]);
                string pathImagen = datos[4];
                int stock = int.Parse(datos[5]);

                Producto producto;

                if(pathImagen != Protocol.NoImage)
                {
                    // Creamos el producto con la info obtenida
                    producto = new (nombre, descripcion, precio, stock, DameNombreImagen(pathImagen));
                    // Recibimos la imagen
                    fileCommsHandler.ReceiveFile(_filesPath);
                } else
                {
                    // Creamos el producto sin imagen
                    producto = new (nombre, descripcion, precio, stock);
                }

                // Llamamos a la lógica para publicarlo
                _productLogic.publicarProducto(producto, user);
                mensajeAlCliente = "Producto ingresado con exito: " + nombre;
            }
            catch (Exception e)
			{
                mensajeAlCliente = "Hubo un error: " + e.Message;
			}
            return mensajeAlCliente;
        }

        public string eliminarProducto(MessageCommsHandler msgHandler) {
            string retorno = "";
            try {
                string[] datos = msgHandler.ReceiveMessage().Split("#");
                string username = datos[0];
                string nombreProd = datos[1];

                Producto eliminado = _productLogic.eliminarProducto(nombreProd, username);
                if (eliminado.Imagen != Protocol.NoImage) BorrarImagen(_filesPath, eliminado.Imagen);

                retorno = "Se ha eliminado exitosamente el producto: "+ eliminado.Nombre;
            }
            catch (Exception e)
            {
                retorno = "Hubo un error: " + e.Message;
            }
            return retorno;
        
        }

        public string productosBuscados(MessageCommsHandler msgHandler)
        {
            int i = 1;
            List<Producto> listaProd = new List<Producto>();
            StringBuilder retorno = new StringBuilder();
            try {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();

                // Buscamos el prodcuto con la informacion
                listaProd = _productLogic.BuscarProductos(nombreProd);

                foreach (Producto producto in listaProd)
                {
                    retorno.AppendLine(i + "- " + producto.Nombre);
                    i++;
                }
                return retorno.ToString();

            }
            catch (Exception e)
            {
                return "Hubo un error: " + e.Message;
            }
            
        }

        public string verMasProducto(MessageCommsHandler msgHandler)
        {
            
            StringBuilder retorno = new StringBuilder();
            try
            {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();
                bool vaImagen = false;

                // Buscamos el prodcuto con la informacion
                Producto p = _productLogic.VerMasProducto(nombreProd);
                if (p.Imagen != Protocol.NoImage)
                {
                    vaImagen = true;
                    retorno.AppendLine("1#"+p.Imagen+"#"); // indicamos que va a recibir imagen

                }
                else
                {
                    retorno.AppendLine("0# #"); // indicamos que no va imagen
                }
                retorno.AppendLine("Nombre: " + p.Nombre);
                retorno.AppendLine("Descripcion: " + p.Descripcion);
                retorno.AppendLine("Precio: " + p.Precio.ToString());
                if(vaImagen) retorno.AppendLine("Nombre de imagen: " + p.Imagen);
                retorno.AppendLine("Stock: " + p.Stock.ToString());

                if(p.calificaciones.Count > 0)
                {
                    retorno.AppendLine("Promedio de calificaciones: " + p.promedioCalificaciones);
                    retorno.AppendLine("Calificaciones: ");
                    foreach (Calificacion cal in p.calificaciones)
                    {
                        retorno.AppendLine("Puntaje: " + cal.puntaje + ". Comentario: " + cal.comentario);
                    }
                } else
                {
                    retorno.AppendLine("El producto aun no ha sido calificado");
                }
                
                return retorno.ToString();
            }
            catch (Exception e)
            {
                return "Hubo un error: " + e.Message;
            }
            

        }

        private string DameNombreImagen(string imagen)
        {
           FileHandler _fileHandeler = new FileHandler();

           return _fileHandeler.GetFileName(imagen);
        }

        public string productosComprados(MessageCommsHandler msgHandler)
        {
            StringBuilder retorno = new StringBuilder();
            try
            {
                // Recibimos el user para mostrarle sus productos comprados
                string user = msgHandler.ReceiveMessage();
                Usuario u = _userLogic.buscarUsuario(user);

                List<Producto> productos = _userLogic.ProductosComprados(u);

                retorno.AppendLine("Sus productos comprados:");
                foreach (Producto prod in productos)
                {
                    retorno.AppendLine(" - " + prod.Nombre);
                }

                return retorno.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string calificarProducto(MessageCommsHandler msgHandler)
        {
            string mensajeAlCliente = "";
            try
            {
                // Capturamos la informacion
                string info = msgHandler.ReceiveMessage();
                string[] datos = info.Split("#");
                string user = datos[0];
                string nombreProd = datos[1];
                string puntaje = datos[2];
                string comentario = datos[3];

                mensajeAlCliente = _productLogic.calificarProducto(user, nombreProd, puntaje, comentario);
            }
            catch (Exception e)
            {
                mensajeAlCliente = "Hubo un error: " + e.Message;
            }
            return mensajeAlCliente;
        }

        private void BorrarImagen(string pathImagenesGuardadas, string nombreImagen)
        {
            FileHandler _fileHandler = new FileHandler();
            _fileHandler.DeleteFile(pathImagenesGuardadas+nombreImagen);
        }
    }
}

