using System;
using System.Net.Sockets;
using System.Net;
using Communication;
using ServerApp.Controllers;
using ServerApp.Domain;

namespace ServerApp
{
    public class ProgramServer
    {
        static readonly SettingsManager settingsMngr = new SettingsManager();
        private static ProductController _productController = new ProductController();

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
            MessageCommsHandler msgHandler = new MessageCommsHandler(socketCliente);
            FileCommsHandler fileHandler = new FileCommsHandler(socketCliente);

            try
            {
                bool clienteConectado = true;

                while (clienteConectado)
                {
                    // Leer la selección del cliente
                    string comando = msgHandler.ReceiveMessage();
                    Console.WriteLine("opcion recibida {0}", comando); // debug

                    // Procesar la selección del cliente
                    Usuario user = new Usuario("mail", "clave"); // TODO sacar esto, usar los datos posta que ingresa el cliente
                    ProcesarSeleccion(msgHandler, comando, fileHandler, user);
                }


                Console.WriteLine("Cliente {0} Desconectado", nro);
            }
            catch (SocketException)
            {
                Console.WriteLine("Cliente {0} Desconectado por un error", nro);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ProcesarSeleccion(MessageCommsHandler msgHandler, string opcion, FileCommsHandler fileHandler, Usuario user)
        {
            switch (opcion)
            {
                case "0":
                    //TODO
                    Console.WriteLine("El cliente entro a la opcion 0"); //debug
                    string datosLogin = msgHandler.ReceiveMessage();
                    Console.WriteLine("Datos de login " + datosLogin); //debug
                    //logica de login
                    msgHandler.SendMessage("Login exitoso");
                    break;
                case "1":
                    Console.WriteLine("entramos a la opcion 1"); //debug
                    msgHandler.SendMessage(_productController.publicarProducto(msgHandler, fileHandler, user));
                    break;
                case "2":
                    // Implementa la lógica para comprar un producto
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Comprar un producto: Implementa la lógica aquí."));
                    break;
                case "3":
                    // Implementa la lógica para modificar un producto publicado
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Modificar un producto: Implementa la lógica aquí."));
                    break;
                case "4":
                    // Implementa la lógica para eliminar un producto
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Eliminar un producto: Implementa la lógica aquí."));
                    break;
                case "5":
                    // Implementa la lógica para buscar un producto
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Buscar un producto: Implementa la lógica aquí."));
                    break;
                case "6":
                    // Implementa la lógica para ver más acerca de un producto
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Ver más acerca de un producto: Implementa la lógica aquí."));
                    break;
                case "7":
                    // Implementa la lógica para calificar un producto
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Calificar un producto: Implementa la lógica aquí."));
                    break;
                default:
                    // Opción no válida
                    //socketCliente.Send(Encoding.UTF8.GetBytes("Opción no válida. Intente nuevamente."));
                    break;
            }
        }

        public static Usuario Autenticar(string email, string password)
        {
            // TODO
            // para mover a un controller de login/logout con logica de usuario
            // tirar excepcion si no existe
            return new Usuario(email, password);
        }
    }
}

