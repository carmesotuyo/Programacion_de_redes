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

            try
            {
                Console.WriteLine(GetMenu());
                while (!parar)
                {
                    string comando = Console.ReadLine();

                    switch (comando)
                    {
                        case "0":
                            msgHandler.SendMessage("0");
                            msgHandler.SendMessage(Login.PedirDatosLogin());
                            Console.WriteLine(msgHandler.ReceiveMessage());
                            break;
                        case "1":
                            //TODO:
                            // pasar a otra carpeta mas prolija despues
                            //agregar validacion para que no se puedan ingresar simbolos especiales -> #
                            // validar en precio y stock que los tipos de datos se pueden parsear a int/float segun corresponde

                            Console.WriteLine("Seleccionó la opción 1: Publicar un producto");

                            // Le pedimos la información al cliente
                            Console.WriteLine("Ingrese nombre del producto");
                            string nombre = Console.ReadLine();

                            Console.WriteLine("Ingrese una descripción para su producto");
                            string descripcion = Console.ReadLine();

                            Console.WriteLine("Ingrese el precio");
                            string precio = Console.ReadLine();

                            Console.WriteLine("Ingrese la ruta al archivo de imagen");
                            string imagen = Console.ReadLine();

                            Console.WriteLine("Ingrese el stock disponible");
                            string stock = Console.ReadLine();

                            //Mandamos al server el comando
                            msgHandler.SendMessage("1");

                            //Mandamos al server la informacion
                            string info = nombre + "#" + descripcion + "#" + precio + "#" + imagen + "#" + stock;
                            msgHandler.SendMessage(info);

                            //Mandamos al server el archivo de imagen
                            fileHandler.SendFile(imagen);

                            // Esperamos exito o error del server
                            Console.WriteLine(msgHandler.ReceiveMessage());

                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "2":
                            Console.WriteLine("Seleccionó la opción 2: Comprar un producto");
                            // Implementa la lógica para comprar un producto aquí
                            break;
                        case "3":
                            Console.WriteLine("Seleccionó la opción 3: Modificar un producto publicado");
                            // Implementa la lógica para modificar un producto aquí
                            break;
                        case "4":
                            Console.WriteLine("Seleccionó la opción 4: Eliminar un producto");
                            // Implementa la lógica para eliminar un producto aquí
                            break;
                        case "5":
                            Console.WriteLine("Seleccionó la opción 5: Buscar un producto");
                            // Implementa la lógica para buscar un producto aquí
                            break;
                        case "6":
                            Console.WriteLine("Seleccionó la opción 6: Ver más acerca de un producto");
                            // Implementa la lógica para ver más acerca de un producto aquí
                            break;
                        case "7":
                            Console.WriteLine("Seleccionó la opción 7: Calificar un producto");
                            // Implementa la lógica para calificar un producto aquí
                            break;
                        case "desconectar":
                            parar = true;
                            Console.WriteLine("Desconectando");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Ingrese un valor dentro las opciones indicadas previamente.");
                            break;
                    }
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



        private static string GetMenu()
        {
            // Define el menú y devuelve su representación en cadena
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("****************************");
            menu.AppendLine("Bienvenido a dreamteam Shop");
            menu.AppendLine("* Seleccione 0 para iniciar sesion");
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
    }
}
