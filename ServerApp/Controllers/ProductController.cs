using System;
using System.Net.Sockets;
using System.Text;
using Communication;

namespace ServerApp.Controllers
{
	public class ProductController
	{

		public ProductController()
		{
		}

		public void publicarProducto(Socket socketCliente)
        {
            byte[] buffer = new byte[Protocol.MaxPacketSize];
            try
			{
                // metodos que le piden al cliente que le mande la info y creo el objeto producto
                socketCliente.Send(Encoding.UTF8.GetBytes("Ingrese nombre del producto"));
                int bytesReceived = socketCliente.Receive(buffer);
                string nombre = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            }
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }

    }
}

