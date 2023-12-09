using Grpc.Net.Client;
using System;

namespace Correo
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5239");
            var client = new Correo.CorreoClient(channel);

            // Aquí deberías tener la lógica para obtener el destinatario y el mensaje
            var destinatario = "usuario@example.com";
            var mensaje = "Este es el cuerpo del correo...";

            // Llama al método EnviarCorreo del servicio de correo
            var response = client.EnviarCorreo(new EnviarCorreoRequest { Destinatario = destinatario, Mensaje = mensaje });

            Console.WriteLine($"Respuesta del servidor: {response.Resultado}");
        }
    }
}
