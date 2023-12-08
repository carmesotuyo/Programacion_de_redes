using Grpc.Core;
using System.Threading.Tasks;

namespace Correo
{
    public class CorreoServiceImpl : Correo.CorreoBase
    {
        public override Task<EnviarCorreoResponse> EnviarCorreo(EnviarCorreoRequest request, ServerCallContext context)
        {
            // Simula el envío de correo con un tiempo de espera de 5 segundos
            Console.WriteLine($"Enviando correo a {request.Destinatario}");
            Task.Delay(5000).Wait();
            Console.WriteLine("Correo enviado.");

            return Task.FromResult(new EnviarCorreoResponse { Resultado = "Correo enviado con éxito." });
        }
    }
}

