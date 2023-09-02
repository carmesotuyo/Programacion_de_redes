using System;
using System.Net;
using System.Net.Sockets;

namespace Server;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server");

            // estamos programando del lado del servidor:
            // Paso 1: Bindear el socket

            // creo el socket del servidor
            var socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // abro el endpoint y le indico la ip y el puerto
            var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);

            // tengo el socket y el endpoint y lo bindeo
            socketServer.Bind(localEndpoint);


            // Paso 2: poner el socket en modo escucha
            socketServer.Listen(1); // 1 es el largo de la cola que pueden estar en espera, cantidad de clientes en espera

            // esperando por clientes
            while (true) // acceptConnections, !salir, etc, variable que defina si el server esta cerrado o no
            { 
                // Paso 3: aceptar conexion
            // me crea una copia del socket al aceptar por donde lo va a atender
            var socketClient = socketServer.Accept(); // es bloqueante, se queda esperando hasta que se conecta
            // Lanzo un hilo para atender a cada cliente
                new Thread(() => HandleClient(socketClient)).Start();
                Console.WriteLine("Acepte un pedido de conexion");
            }
            Console.ReadLine();
        }

    static void HandleClient(Socket client) {
        bool isConnected = true;
        while (isConnected)
        {
            Thread.Sleep(5000);
            isConnected = false;
        }
        Console.WriteLine("Cliente desconectado");
    }
}