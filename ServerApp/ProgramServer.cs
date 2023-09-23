using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using Communication;
using System.Text;
using System.Runtime.InteropServices;

namespace ServerApp
{
    public class ProgramServer
    {
        static readonly SettingsManager settingsMngr = new SettingsManager();
        const int bufferNumber = 1024;
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
            Console.WriteLine("Cliente {0} conectado", nro);

            byte[] buffer = new byte[bufferNumber];
            try
            {
                socketCliente.Send(Encoding.UTF8.GetBytes("Porfavor, ingrese su email"));
                int bytesRecived = socketCliente.Receive(buffer);
                string email = Encoding.UTF8.GetString(buffer,0,bytesRecived);

                socketCliente.Send(Encoding.UTF8.GetBytes("Porfavor, ingrese su contraseña"));
                bytesRecived = socketCliente.Receive(buffer);
                string password = Encoding.UTF8.GetString(buffer, 0, bytesRecived);

                if (Autenticar(email, password)) {
                    socketCliente.Send(Encoding.UTF8.GetBytes("Bienvenido a dream team market, autenticacion exitosa"));
                    bool clienteConectado = true;

                    while (clienteConectado)
                    {
                        // Mostrar el menú al cliente
                        socketCliente.Send(Encoding.UTF8.GetBytes(GetMenu()));

                        // Leer la selección del cliente
                        bytesRecived = socketCliente.Receive(buffer);
                        string opcion = Encoding.UTF8.GetString(buffer, 0, bytesRecived);

                        // Procesar la selección del cliente
                        ProcesarSeleccion(socketCliente, opcion);


                        /// RECIBO EL ARCHIVO /////
                        // Console.WriteLine("Bienvenido a dream team market");
                        // Console.WriteLine("Antes de recibir el archivo");
                        //var fileCommonHandler = new FileCommsHandler(socketCliente);
                        //fileCommonHandler.ReceiveFile();
                        //Console.WriteLine("Archivo recibido!!");
                    }
                }
                

                Console.WriteLine("Cliente Desconectado");
            }
            catch (SocketException)
            {
                Console.WriteLine("Cliente Desconectado!");
            }
        }

        private static void ProcesarSeleccion(Socket socketCliente, string opcion)
        {
            // Aquí debes implementar la lógica para cada opción del menú
            // Por ejemplo, puedes usar un switch para manejar cada opción
            switch (opcion)
            {
                case "1":
                    // Implementa la lógica para publicar un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Publicar un producto: Implementa la lógica aquí."));
                    break;
                case "2":
                    // Implementa la lógica para comprar un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Comprar un producto: Implementa la lógica aquí."));
                    break;
                case "3":
                    // Implementa la lógica para modificar un producto publicado
                    socketCliente.Send(Encoding.UTF8.GetBytes("Modificar un producto: Implementa la lógica aquí."));
                    break;
                case "4":
                    // Implementa la lógica para eliminar un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Eliminar un producto: Implementa la lógica aquí."));
                    break;
                case "5":
                    // Implementa la lógica para buscar un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Buscar un producto: Implementa la lógica aquí."));
                    break;
                case "6":
                    // Implementa la lógica para ver más acerca de un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Ver más acerca de un producto: Implementa la lógica aquí."));
                    break;
                case "7":
                    // Implementa la lógica para calificar un producto
                    socketCliente.Send(Encoding.UTF8.GetBytes("Calificar un producto: Implementa la lógica aquí."));
                    break;
                default:
                    // Opción no válida
                    socketCliente.Send(Encoding.UTF8.GetBytes("Opción no válida. Intente nuevamente."));
                    break;
            }
        }

        private static string GetMenu()
        {
            // Define el menú y devuelve su representación en cadena
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("****************************");
            menu.AppendLine("Bienvenido a dreamteam Shop");
            menu.AppendLine("* Seleccione 1 para publicar un producto");
            menu.AppendLine("* Seleccione 2 para comprar un producto");
            menu.AppendLine("* Seleccione 3 para modificar un producto publicado");
            menu.AppendLine("* Seleccione 4 para eliminar un producto");
            menu.AppendLine("* Seleccione 5 para buscar un producto");
            menu.AppendLine("* Seleccione 6 para ver más acerca de un producto");
            menu.AppendLine("* Seleccione 7 para calificar un producto");
            menu.AppendLine("Muchas gracias por elegirnos!");
            menu.AppendLine("****************************");
            return menu.ToString();
        }

        public static bool Autenticar(string email, string password)
        {
            return true;
        }
    }
}

