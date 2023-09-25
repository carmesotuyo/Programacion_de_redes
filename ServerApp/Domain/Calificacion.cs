namespace ServerApp.Domain
{
	public class Calificacion
	{
		public int puntaje;
		public string? comentario;
		public Producto producto;

		public Calificacion(Producto producto, int puntaje, string comentario = null)
		{
            this.producto = producto;
			this.puntaje = puntaje;
			if(comentario != null)
			{
                this.comentario = comentario;
            }
		}
	}
}

