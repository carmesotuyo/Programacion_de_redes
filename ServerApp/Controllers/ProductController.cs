using System;
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

    }
}

