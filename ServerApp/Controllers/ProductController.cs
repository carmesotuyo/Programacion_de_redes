using System;
using System.Net.Sockets;
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

		public void publicarProducto(MessageCommsHandler msgHandler, FileCommsHandler fileHandler, Usuario user)
        {
            try
			{
                Console.WriteLine("entramos al controller"); //debug
                // Le pedimos la información al cliente
                msgHandler.SendMessage("Ingrese nombre del producto");
                string nombre = msgHandler.ReceiveMessage();

                msgHandler.SendMessage("Ingrese una descripción para su producto");
                string descripcion = msgHandler.ReceiveMessage();

                msgHandler.SendMessage("Ingrese el precio");
                float precio = msgHandler.ReceiveNumber();

                msgHandler.SendMessage("Ingrese la ruta al archivo de imagen");
                string imagen = fileHandler.ReceiveFile();

                msgHandler.SendMessage("Ingrese el stock disponible");
                int stock = (int)msgHandler.ReceiveNumber();

                // Creamos el producto con la info obtenida
                Producto producto = new Producto(nombre, descripcion, precio, imagen, stock);

                // Llamamos a la lógica para publicarlo
                _productLogic.publicarProducto(producto, user);
            }
            catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }

    }
}

