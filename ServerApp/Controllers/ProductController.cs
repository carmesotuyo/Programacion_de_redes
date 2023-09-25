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

        public string modificarProducto(MessageCommsHandler msgHandler, Usuario user) {

            string mensajeACliente = "";
            string info = msgHandler.ReceiveMessage();

            string[] datos = info.Split('#');
            string nombreProd = datos[0];
            string atributoAModificar = datos[1].ToLower();
            string nuevoValor = datos[2];
            if (_productLogic.buscarProductoPorNombre(nombreProd).Count > 0)
            {
                Producto p = _productLogic.buscarProductoPorNombre(nombreProd)[0];

                switch (atributoAModificar)
                {
                    case "nombre":
                        if (_productLogic.buscarProductoPorNombre(nuevoValor).Count == 0)
                        {
                            p.Nombre = nuevoValor;
                            mensajeACliente = "Nombre del producto actualizado con éxito.";
                        }
                        else
                        {
                            mensajeACliente = "El nuevo nombre ya es utilizado por otro producto";
                        }

                        break;

                    case "descripcion":
                        p.Descripcion = nuevoValor;
                        _productLogic.modificarProducto(p, nombreProd);
                        mensajeACliente = "Descripción del producto actualizada con éxito.";
                        break;

                    case "precio":
                        if (float.TryParse(nuevoValor, out float nuevoPrecio))
                        {
                            p.Precio = nuevoPrecio;
                            _productLogic.modificarProducto(p, nombreProd);
                            mensajeACliente = "Precio del producto actualizado con éxito.";
                        }
                        else
                        {
                            mensajeACliente = "El nuevo valor de precio no es válido.";
                        }
                        break;

                    case "imagen":
                        p.Imagen = nuevoValor;
                        _productLogic.modificarProducto(p, nombreProd);
                        mensajeACliente = "Imagen del producto actualizada con éxito.";
                        break;

                    case "stock":
                        if (int.TryParse(nuevoValor, out int nuevoStock))
                        {

                            p.Stock = nuevoStock;
                            _productLogic.modificarProducto(p, nombreProd);
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
            }
            else {
                mensajeACliente = "El producto ingresado no existe :(";
            }
            
            return mensajeACliente;

        }
		public string publicarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileHandler, Usuario user)
        {
            string mensajeAlCliente = "";
            try
			{
                // Capturamos la informacion
                string info = msgHandler.ReceiveMessage();
                string[] datos = info.Split("#");
                string nombre = datos[0];
                string descripcion = datos[1];
                float precio = float.Parse(datos[2]);
                string imagen = datos[3];
                int stock = int.Parse(datos[4]);

                Producto producto;

                if(imagen != Protocol.NoImagePath)
                {
                    // Creamos el producto con la info obtenida
                    producto = new (nombre, descripcion, precio, stock, imagen);
                    // Recibimos la imagen
                    fileHandler.ReceiveFile();
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
				Console.WriteLine(e.Message);
                mensajeAlCliente = "Ocurrió un error: " + e.Message;
			}
            return mensajeAlCliente;
        }
        public string eliminarProducto(MessageCommsHandler msgHandler, Usuario user) {
            string retorno = "";
            try {
                string nombreProd = msgHandler.ReceiveMessage();
                retorno = "Se ha eliminado exitosamente el producto: "+ _productLogic.eliminarProducto(nombreProd).Nombre;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                retorno = "Ocurrió un error: " + e.Message;
            }
            return retorno;
        
        }
        public string productosBuscados(MessageCommsHandler msgHandler, Usuario user) {
            int i = 1;
            List<Producto> listaProd = new List<Producto>();
            StringBuilder retorno = new StringBuilder();
            try {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();
                // Buscamos el prodcuto con la informacion
                listaProd = _productLogic.buscarProductoPorNombre(nombreProd);
                if (listaProd.Count > 0)
                {
                    foreach (Producto producto in listaProd)
                    {
                        retorno.AppendLine(i + "- " + producto.Nombre);
                        i++;
                    }
                    return retorno.ToString();
                }
                else {
                    string ret = "No existen productos con ese nombre";
                    return ret;
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                string ret = "Ocurrió un error: " + e.Message;
                return ret;
            }
            
        }
        public string verMasProducto(MessageCommsHandler msgHandler, Usuario user)
        {
            
            StringBuilder retorno = new StringBuilder();
            try
            {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();
                // Buscamos el prodcuto con la informacion
                if (_productLogic.buscarProductoPorNombre(nombreProd).Count > 0)
                {
                    Producto p = _productLogic.buscarProductoPorNombre(nombreProd)[0];
                    retorno.AppendLine("Nombre: " + p.Nombre);
                    retorno.AppendLine("Descripcion: " + p.Descripcion);
                    retorno.AppendLine("Precio: " + p.Precio.ToString());
                    retorno.AppendLine("Ruta de imagen: " + p.Imagen);
                    retorno.AppendLine("Nombre de imagen: " + DameNombreImagen(p.Imagen));
                    retorno.AppendLine("Stock: " + p.Stock.ToString());

                    return retorno.ToString();
                }
                else {
                    string ret = "El nombre del producto ingresado no existe :(";
                    return ret;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string ret = "Ocurrió un error: " + e.Message;
                return ret;
            }
            

        }

        private string DameNombreImagen(string imagen)
        {
            FileHandler _fileHandeler = new FileHandler();

           return _fileHandeler.GetFileName(imagen);
        }
    }
}

