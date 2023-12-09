using System;
namespace ServerApp.Domain
{
	public class Compra
    {
        public string Usuario;
        public string NombreProducto { get; set; }
        public float Precio { get; set; }
        public DateTime Fecha { get; set; }
        public string MensajeEntregadoACliente { get; set; }

        public Compra(string user, string producto, float precio, DateTime fecha, string mensaje)
		{
            Usuario = user;
            NombreProducto = producto;
            Precio = precio;
            Fecha = fecha;
            MensajeEntregadoACliente = mensaje;
		}

        // Constructor en caso de error en la compra
        public Compra(string mensaje)
        {
            MensajeEntregadoACliente = mensaje;
        }
	}
}

