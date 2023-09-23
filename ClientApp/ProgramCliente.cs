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
            bool parar = false;
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


            MessageCommsHandler msgHandler = new MessageCommsHandler(socketCliente);
            FileCommsHandler fileHandler = new FileCommsHandler(socketCliente);

            //Console.WriteLine("Escribir mensaje y presionar enter para enviar....");
            //byte[] buffer = new byte[1024];

            try
            {
                //int byteReceived  = socketCliente.Receive(buffer);
                //string serverMessage = Encoding.UTF8.GetString(buffer,0,byteReceived);
                //Console.WriteLine(serverMessage);

                //Console.WriteLine("Email: ");
                //string email = Console.ReadLine();
                //socketCliente.Send(Encoding.UTF8.GetBytes(email));

                //byteReceived = socketCliente.Receive(buffer);
                //serverMessage = Encoding.UTF8.GetString(buffer,0,byteReceived);
                //Console.WriteLine(serverMessage);

                //Console.WriteLine("Password: ");
                //string password = Console.ReadLine();
                //socketCliente.Send(Encoding.UTF8.GetBytes(password));

                //byteReceived = socketCliente.Receive(buffer);
                //serverMessage = Encoding.UTF8.GetString(buffer, 0, byteReceived);
                //Console.WriteLine(serverMessage);

                //Console.WriteLine("****************************");
                //Console.WriteLine("Bienvenido a dreamteam Shop");
                //Console.WriteLine("* Seleccione 1 para publicar un producto");
                //Console.WriteLine("* Seleccione 2 para comprar un producto");
                //Console.WriteLine("* Seleccione 3 para modificar un producto publicado");
                //Console.WriteLine("* Seleccione 4 para eliminar un producto");
                //Console.WriteLine("* Seleccione 5 para buscar un producto");
                //Console.WriteLine("* Seleccione 6 para ver más acerca de un producto");
                //Console.WriteLine("* Seleccione 7 para calificar un producto");
                //Console.WriteLine("Muchas gracias por elegirnos!");
                //Console.WriteLine("****************************");

                while (!parar)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("recibiendo algo"); //debug
                    Console.WriteLine(msgHandler.ReceiveMessage());
                    Console.WriteLine("enviando algo"); //debug
                    string clientMsg = Console.ReadLine();
                    if(clientMsg == "parar")
                    {
                        parar = true;
                        msgHandler.SendMessage("Cierro la conexión");
                    } else
                    {
                        msgHandler.SendMessage(clientMsg);
                    }
                    //    //Console.WriteLine("Ingrese la ruta completa del archivo a enviar: ");
                    //    //String abspath = Console.ReadLine();
                    //    // var fileCommonHandler = new FileCommsHandler(socketCliente);
                    //    //fileCommonHandler.SendFile(abspath);
                    //    //Console.WriteLine("Se envio el archivo al Servidor");
                    //    Console.WriteLine("Seleccione una opcion: ");
                    //    string opcion = Console.ReadLine();
                    //    switch (opcion)
                    //    {
                    //        case "1":
                    //            Console.WriteLine("Seleccionó la opción 1: Publicar un producto");
                    //            // Implementa la lógica para publicar un producto aquí
                    //            break;
                    //        case "2":
                    //            Console.WriteLine("Seleccionó la opción 2: Comprar un producto");
                    //            // Implementa la lógica para comprar un producto aquí
                    //            break;
                    //        case "3":
                    //            Console.WriteLine("Seleccionó la opción 3: Modificar un producto publicado");
                    //            // Implementa la lógica para modificar un producto aquí
                    //            break;
                    //        case "4":
                    //            Console.WriteLine("Seleccionó la opción 4: Eliminar un producto");
                    //            // Implementa la lógica para eliminar un producto aquí
                    //            break;
                    //        case "5":
                    //            Console.WriteLine("Seleccionó la opción 5: Buscar un producto");
                    //            // Implementa la lógica para buscar un producto aquí
                    //            break;
                    //        case "6":
                    //            Console.WriteLine("Seleccionó la opción 6: Ver más acerca de un producto");
                    //            // Implementa la lógica para ver más acerca de un producto aquí
                    //            break;
                    //        case "7":
                    //            Console.WriteLine("Seleccionó la opción 7: Calificar un producto");
                    //            // Implementa la lógica para calificar un producto aquí
                    //            break;
                    //        default:
                    //            Console.WriteLine("Opción no válida. Intente nuevamente.");
                    //            break;
                    //    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de comunicacion: " + ex);
            }
            finally {
                Console.WriteLine("Cierro el Cliente");
                // Cerrar la conexion.
                socketCliente.Shutdown(SocketShutdown.Both);
                socketCliente.Close();
            }        
        }
    }
}
