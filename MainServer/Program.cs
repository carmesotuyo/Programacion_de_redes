using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MainServer.Services;

namespace MainServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración del servidor gRPC
            builder.Services.AddGrpc();
            var app = builder.Build();

            // Configuración del servidor de correo
            var correoChannel = GrpcChannel.ForAddress("http://localhost:5239");
            var correoClient = new Correo.Greeter.GreeterClient(correoChannel);

            // Configuración del servidor principal
            app.MapGrpcService<GreeterService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

            app.Run();
        }
    }
}



