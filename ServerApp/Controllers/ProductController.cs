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

                // Creamos el producto con la info obtenida
                Producto producto = new Producto(nombre, descripcion, precio, imagen, stock);

                fileHandler.ReceiveFile();

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
        public string productosBuscados(MessageCommsHandler msgHandler, FileCommsHandler fileHandler, Usuario user) {
            int i = 1;
            List<Producto> listaProd = new List<Producto>();
            StringBuilder retorno = new StringBuilder();
            try {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();
                // Buscamos el prodcuto con la informacion
                listaProd = _productLogic.buscarProductoPorNombre(nombreProd);
                foreach (Producto producto in listaProd)
                {
                    retorno.AppendLine(i+"- " + producto.Nombre);
                    i++;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Ocurrio un error: ", e.Message);
            }
            return retorno.ToString();

        }
        public string verMasProducto(MessageCommsHandler msgHandler, FileCommsHandler fileHandler, Usuario user)
        {
            
            StringBuilder retorno = new StringBuilder();
            try
            {
                // Capturamos la informacion
                string nombreProd = msgHandler.ReceiveMessage();
                // Buscamos el prodcuto con la informacion
                Producto p = _productLogic.buscarProductoPorNombre(nombreProd)[0];
                retorno.AppendLine("Nombre: "+p.Nombre);
                retorno.AppendLine("Descripcion: "+p.Descripcion);
                retorno.AppendLine("Precio: "+p.Precio.ToString());
                retorno.AppendLine("Ruta de imagen: " + p.Imagen);
                retorno.AppendLine("Stock: " + p.Stock.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: ", e.Message);
            }
            return retorno.ToString();

        }


    }
}

