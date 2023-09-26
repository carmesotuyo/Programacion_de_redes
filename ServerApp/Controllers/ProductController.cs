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

		public ProductController() { }

        public string modificarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileHandler, string filesPath) {

            string mensajeACliente = "";
            try
            {
                string info = msgHandler.ReceiveMessage();
                string[] datos = info.Split('#');
                string username = datos[0];
                string nombreProd = datos[1];
                string atributoAModificar = datos[2].ToLower();
                string nuevoValor = datos[3];

                Producto p = _productLogic.BuscarProductos(nombreProd)[0];

                switch (atributoAModificar)
                {
                    case "nombre":
                        _productLogic.ValidarNombreRepetido(nuevoValor);
                        p.Nombre = nuevoValor;
                        mensajeACliente = "Nombre del producto actualizado con éxito.";
                        break;

                    case "descripcion":
                        p.Descripcion = nuevoValor;
                        mensajeACliente = "Descripción del producto actualizada con éxito.";
                        break;

                    case "precio":
                        if (float.TryParse(nuevoValor, out float nuevoPrecio))
                        {
                            p.Precio = nuevoPrecio;
                            mensajeACliente = "Precio del producto actualizado con éxito.";
                        }
                        else
                        {
                            mensajeACliente = "El nuevo valor de precio no es válido.";
                        }
                        break;

                    case "imagen":
                        p.Imagen = DameNombreImagen(nuevoValor);
                        string imagenAnterior = _productLogic.CambiarImagen(p, username);
                        if (imagenAnterior != Protocol.NoImage) BorrarImagen(filesPath, nombreProd);
                        fileHandler.ReceiveFile(filesPath);
                        mensajeACliente = "Imagen del producto actualizada con éxito.";
                        break;

                    case "stock":
                        if (int.TryParse(nuevoValor, out int nuevoStock))
                        {
                            p.Stock = nuevoStock;
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

                _productLogic.modificarProducto(p, nombreProd, username);
            }
            catch(Exception e)
            {
                mensajeACliente = e.Message;
            }
            
            return mensajeACliente;

        }

		public string publicarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileCommsHandler, string filesPath)
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
                    fileCommsHandler.ReceiveFile(filesPath);
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
                mensajeAlCliente = "Ocurrió un error: " + e.Message;
			}
            return mensajeAlCliente;
        }

        public string eliminarProducto(MessageCommsHandler msgHandler) {
            string retorno = "";
            try {
                string[] datos = msgHandler.ReceiveMessage().Split("#");
                string username = datos[0];
                string nombreProd = datos[1];
                retorno = "Se ha eliminado exitosamente el producto: "+ _productLogic.eliminarProducto(nombreProd, username).Nombre;
            }
            catch (Exception e)
            {
                retorno = "Ocurrió un error: " + e.Message;
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
            catch (Exception e) {
                return "Ocurrió un error: " + e.Message;
            }
            
        }

        public string verMasProducto(MessageCommsHandler msgHandler)
        {
            
            StringBuilder retorno = new StringBuilder();
            try
            {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();

                // Buscamos el prodcuto con la informacion
                Producto p = _productLogic.VerMasProducto(nombreProd);
                retorno.AppendLine("Nombre: " + p.Nombre);
                retorno.AppendLine("Descripcion: " + p.Descripcion);
                retorno.AppendLine("Precio: " + p.Precio.ToString());
                if(p.Imagen != null) retorno.AppendLine("Nombre de imagen: " + DameNombreImagen(p.Imagen));
                retorno.AppendLine("Stock: " + p.Stock.ToString());

                return retorno.ToString();
            }
            catch (Exception e)
            {
                return "Ocurrió un error: " + e.Message;
            }
            

        }

        private string DameNombreImagen(string imagen)
        {
           FileHandler _fileHandeler = new FileHandler();

           return _fileHandeler.GetFileName(imagen);
        }

        private void BorrarImagen(string pathImagenesGuardadas, string nombreImagen)
        {
            FileHandler _fileHandler = new FileHandler();
            _fileHandler.DeleteFile(pathImagenesGuardadas+"/"+nombreImagen);
        }
    }
}

