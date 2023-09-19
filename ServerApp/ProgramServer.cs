using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using Communication;
using System.Text;

namespace ServerApp
{
    public class ProgramServer
    {
        static readonly SettingsManager settingsMngr = new SettingsManager();
        public static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Aplicacion Servidor....!!!");

            var socketServer = new Socket(
            AddressFamily.InterNetwork,
                SocketType.Stream,
                    ProtocolType.Tcp);

            string ipServer = settingsMngr.ReadSettings(ServerConfig.serverIPconfigkey);
            int ipPort = int.Parse(settingsMngr.ReadSettings(ServerConfig.serverPortconfigkey));

            var localEndpoint = new IPEndPoint(IPAddress.Parse(ipServer), ipPort);
            // puertos 0 a 65535   pero del 1 al 1024 estan reservados  

            socketServer.Bind(localEndpoint); // vinculo el socket al EndPoint
            socketServer.Listen(2); // Pongo al Servidor en modo escucha
            int clientes = 0;
            bool salir = false;


            while (!salir)
            {
                var socketClient = socketServer.Accept();
                clientes++;
                int nro = clientes;
                Console.WriteLine("Acepte un nuevo pedido de Conexion");
                new Thread(() => ManejarCliente(socketClient, nro)).Start();

            }

            Console.ReadLine();

            // Cierro el socket
            socketServer.Shutdown(SocketShutdown.Both);
            socketServer.Close();

        }

        static void ManejarCliente(Socket socketCliente, int nro)
        {
            try
            {
                Console.WriteLine("Cliente {0} conectado", nro);
                bool clienteConectado = true;
                while (clienteConectado)
                {
                    /// RECIBO EL ARCHIVO /////

                    Console.WriteLine("Antes de recibir el archivo");
                    var fileCommonHandler = new FileCommsHandler(socketCliente);
                    string path = settingsMngr.ReadSettings(ServerConfig.imagePathconfigkey);
                    fileCommonHandler.ReceiveFile(path);
                    Console.WriteLine("Archivo recibido!!");
                }
                Console.WriteLine("Cliente Desconectado");
            }
            catch (SocketException)
            {
                Console.WriteLine("Cliente Desconectado!");
            }
        }

    }
}

