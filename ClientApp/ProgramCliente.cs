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
            bool estaAutenticado = false;
            string user = "";
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
                            string credenciales = Login.PedirDatosLogin();
                            msgHandler.SendMessage(credenciales);

                            string respuesta = msgHandler.ReceiveMessage();

                            if (respuesta == "1")
                            {
                                Console.WriteLine("Login exitoso");
                                estaAutenticado = true;
                                user = credenciales.Split("#")[0];
                            }
                            else
                            {
                                Console.WriteLine("Usuario o contraseña incorrecta");
                            }
                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "1":
                            //TODO:
                            // pasar a otra carpeta mas prolija despues
                            //agregar validacion para que no se puedan ingresar simbolos especiales -> #
                            // validar en precio y stock que los tipos de datos se pueden parsear a int/float segun corresponde

                            if (estaAutenticado)
                            {
                                Console.WriteLine("Seleccionó la opción 1: Publicar un producto");

                                // Le pedimos la información al cliente
                                Console.WriteLine("Ingrese nombre del producto");
                                string nombre = Console.ReadLine();

                                Console.WriteLine("Ingrese una descripción para su producto");
                                string descripcion = Console.ReadLine();

                                Console.WriteLine("Ingrese el precio");
                                string precio = Console.ReadLine();

                                Console.WriteLine("Desea ingresar una imagen? Responda 'si' para cargar imagen, enter para seguir sin subir imagen");
                                bool subeImagen = false;
                                string imagen = Protocol.NoImage;

                                if(Console.ReadLine() == "si")
                                {
                                    subeImagen = true;
                                    Console.WriteLine("Ingrese la ruta al archivo de imagen");
                                    imagen = Console.ReadLine();
                                }

                                Console.WriteLine("Ingrese el stock disponible");
                                string stock = Console.ReadLine();

                                //Mandamos al server el comando
                                msgHandler.SendMessage("1");

                                //Mandamos al server la informacion
                                string info = user + "# "+ nombre + "#" + descripcion + "#" + precio + "#" + imagen + "#" + stock;
                                msgHandler.SendMessage(info);

                                if (subeImagen)
                                {
                                    //Mandamos al server el archivo de imagen
                                    fileHandler.SendFile(imagen);
                                }

                                // Esperamos exito o error del server
                                Console.WriteLine(msgHandler.ReceiveMessage());
                            }
                            else
                            {
                                Console.WriteLine("Para realizar esta acción debes estar logeado");
                            }

                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "2":
                            Console.WriteLine("Seleccionó la opción 2: Comprar un producto");
                            // Implementa la lógica para comprar un producto aquí
                            break;
                        case "3":
                            if (estaAutenticado)
                            {
                                Console.WriteLine("Seleccionó la opción 3: Modificar un producto publicado");
                                Console.WriteLine("Para modificar porfavor ingrese el nombre de producto a modificar");

                                string nombreProducto = Console.ReadLine();

                                Console.WriteLine("Ingrese que atributo quiere modificar");
                                string atributoAModificar = Console.ReadLine();

                                Console.WriteLine("Ingrese el nuevo valor del atributo seleccionado, en caso de la imagen ingrese la ruta completa");
                                string nuevoValorDelAtributo = Console.ReadLine();

                                msgHandler.SendMessage("3");

                                string informacion = user + "#" + nombreProducto + "#" + atributoAModificar + "#" + nuevoValorDelAtributo;

                                msgHandler.SendMessage(informacion);

                                if(atributoAModificar.ToLower() == "imagen")
                                {
                                    fileHandler.SendFile(nuevoValorDelAtributo);
                                }

                                // Esperamos exito o error del server
                                Console.WriteLine(msgHandler.ReceiveMessage());
                            } else
                            {
                                Console.WriteLine("Para realizar esta acción debes estar logeado");
                            }

                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;

                        case "4":
                            if (estaAutenticado)
                            {
                                Console.WriteLine("Seleccionó la opción 4: Eliminar un producto");
                                Console.WriteLine("Para eliminar porfavor ingrese el nombre del producto que quiere eliminar");
                                string nombreProductoABorrar = Console.ReadLine();
                                //Mandamos al server el comando
                                msgHandler.SendMessage("4");
                                //Mandamos al server la informacion
                                msgHandler.SendMessage(user + "#" + nombreProductoABorrar);
                                // Esperamos exito o error del server
                                Console.WriteLine(msgHandler.ReceiveMessage());
                            }
                            else
                            {
                                Console.WriteLine("Para realizar esta acción debes estar logeado");
                            }
                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "5":
                            Console.WriteLine("Seleccionó la opción 5: Buscar un producto");
                            Console.WriteLine("Para buscar porfavor ingrese alguna letra que contenga el nombre del producto que busca");
                            string textoABuscar = Console.ReadLine();
                            //Mandamos al server el comando
                            msgHandler.SendMessage("5");
                            //Mandamos al server la informacion
                            msgHandler.SendMessage(textoABuscar);
                            // Esperamos exito o error del server
                            Console.WriteLine(msgHandler.ReceiveMessage());
                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "6":
                            Console.WriteLine("Seleccionó la opción 6: Ver más acerca de un producto");
                            Console.WriteLine("Para buscar porfavor ingrese nombre del producto que quiere ver mas informacion");
                            string nombreProductoMasInfo = Console.ReadLine();
                            //Mandamos al server el comando
                            msgHandler.SendMessage("6");
                            //Mandamos al server la informacion
                            msgHandler.SendMessage(nombreProductoMasInfo);
                            // Esperamos exito o error del server
                            Console.WriteLine(msgHandler.ReceiveMessage());
                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
                            break;
                        case "7":
                            Console.WriteLine("Seleccionó la opción 7: Calificar un producto");
                            // Implementa la lógica para calificar un producto aquí
                            break;
                        case "8":
                            Console.WriteLine("Seleccionó la opción 8: Ver todos los productos");
                            msgHandler.SendMessage("8");
                            Console.WriteLine(msgHandler.ReceiveMessage());
                            Console.WriteLine("Ingrese un valor del menú principal para realizar otra acción");
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
            menu.AppendLine("* Seleccione 8 para ver el listado de productos");
            menu.AppendLine("Muchas gracias por elegirnos!");
            menu.AppendLine("****************************");
            return menu.ToString();
        }
    }
}
