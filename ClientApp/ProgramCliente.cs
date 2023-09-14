using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Communication;

namespace ClientApp
{
    public class ProgramCliente
    {
        static readonly SettingsManager settingsMngr = new SettingsManager();
        public static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Aplicacion Cliente....!!!");

            var socketCliente = new Socket(
            AddressFamily.InterNetwork,
                SocketType.Stream,
                    ProtocolType.Tcp);

            string ipServer = settingsMngr.ReadSettings(ClientConfig.serverIPconfigkey);
            string ipClient = settingsMngr.ReadSettings(ClientConfig.clientIPconfigkey);
            int serverPort = int.Parse(settingsMngr.ReadSettings(ClientConfig.serverPortconfigkey));

            var localEndPoint = new IPEndPoint(IPAddress.Parse(ipClient), 0);
            socketCliente.Bind(localEndPoint);
            var serverEndpoint = new IPEndPoint(IPAddress.Parse(ipServer), serverPort);
            socketCliente.Connect(serverEndpoint);
            Console.WriteLine("Cliente Conectado al Servidor...!!!");

            //Console.WriteLine("Escribir mensaje y presionar enter para enviar....");
            bool parar = false;
            while (!parar)
            {
                Console.WriteLine("Ingrese la ruta completa del archivo a enviar: ");
                String abspath = Console.ReadLine();
                var fileCommonHandler = new FileCommsHandler(socketCliente);
                fileCommonHandler.SendFile(abspath);
                Console.WriteLine("Se envio el archivo al Servidor");
            }

            Console.WriteLine("Cierro el Cliente");
            // Cerrar la conexion.
            socketCliente.Shutdown(SocketShutdown.Both);
            socketCliente.Close();

        
    }
    }
}
