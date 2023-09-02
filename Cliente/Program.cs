using System.Net;
using System.Net.Sockets;

namespace Client;
class Program
{
    // puedo ejecutar varias veces el client para simular varios clientes

    static void Main(string[] args)
    {
        Console.WriteLine("Starting Client");

        var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // creo el endpoint local
        var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0); // le puedo definir un puerto (15000), o pongo 0 que es un comodin, se conecta al primer puerto disponible

        // hago el bind del local
        socketClient.Bind(localEndpoint);

        // creo el endpoint remoto (del servidor) con la IP y puerto del server
        var remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);

        // conecto el socket local al socket remoto
        socketClient.Connect(remoteEndpoint);

        Console.WriteLine("Cliente conectado.");
        Console.ReadLine();

        // cerrar la conexion
        Console.WriteLine("Voy a cerrar");
        // el shutdown espera a que termine por si hay algo en el buffer pendiente
        socketClient.Shutdown(SocketShutdown.Both);
        socketClient.Close();
    }
}