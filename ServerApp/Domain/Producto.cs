using System;
using System.Drawing;

namespace ServerApp.Domain
{
	public class Producto
	{
		private static int globalIdCounter;
		public int id;
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public float Precio { get; set; }
		public string Imagen { get; set; }
		public int stock;
		public List<Calificacion> calificaciones;
		public int promedioCalificaciones;

		public Producto(string nombre, string descripcion, float precio, string imagen, int stock)
		{
			Nombre = nombre;
			Descripcion = descripcion;
			Precio = precio;
			Imagen = imagen;
			this.stock = stock;
			calificaciones = new List<Calificacion>();
			id = globalIdCounter++;
            globalIdCounter++;
		}

		public int agregarStock(int cantidad)
		{
			stock += cantidad;
			return stock;
        }

        public int quitarStock(int cantidad)
        {
            stock -= cantidad;
            return stock;
        }

		public int actualizarPromedioDeCalificaciones()
		{
			int sumaPuntajes = 0;
			int totalPuntajes = 0;

			foreach(Calificacion calificacion in calificaciones)
			{
				sumaPuntajes += calificacion.puntaje;
				totalPuntajes++;
			}

			return promedioCalificaciones = sumaPuntajes / totalPuntajes;
		}

		public List<Calificacion> agregarCalificacion(Calificacion calificacion)
		{
			if (calificacion.producto == this)
			{
				calificaciones.Add(calificacion);
                return calificaciones;
            }
			else throw new Exception("Esta calificación no pertenece a este producto.");
		}
    }
}

