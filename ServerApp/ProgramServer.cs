using System.Net.Sockets;
using System.Net;
using Communication;
using ServerApp.Controllers;
using ServerApp.Domain;
using System.Collections.Specialized;
using System.Configuration;

namespace ServerApp
{
    public class ProgramServer
    {
        static readonly SettingsManager settingsMngr = new();
        private static string filesPath = settingsMngr.ReadSettings(ServerConfig.imagePathconfigkey);
        private static readonly ProductController _productController = new(filesPath);
        private static readonly UserController _userController = new();
        NameValueCollection usuarios = ConfigurationManager.GetSection("Usuarios") as NameValueCollection;
        NameValueCollection productos = ConfigurationManager.GetSection("Productos") as NameValueCollection;
        private static Dictionary<Socket, bool> clientesConectados = new();
        private static bool salir = false;

        public static void Main(string[] args)
        {
            ProgramServer server = new ProgramServer();
            server.agregarUsuarios();
            server.agregarProductos();
            Console.WriteLine("Iniciando Aplicacion Servidor....!!!");
            Console.WriteLine("Para cerrar este servidor ingrese 'salir' en cualquier momento");
            
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
            new Thread(() => CerrarServidor(socketServer)).Start();

            while (!salir)
            {
                var socketClient = socketServer.Accept();
                clientesConectados.Add(socketClient, true);
                clientes++;
                int nro = clientes;
                Console.WriteLine("Acepte un nuevo pedido de Conexion");
                new Thread(() => ManejarCliente(socketClient, nro)).Start();
            }

            //Console.ReadLine();

            // Cierro el socket
        }

        static void CerrarServidor(Socket socketServer)
        {
            try
            {
                salir = Console.ReadLine() == "salir";
                if (salir)
                {
                    // cerramos todas las conexiones con los clientes
                    foreach (Socket cliente in clientesConectados.Keys)
                    {
                        MessageCommsHandler msgHandler = new(cliente);
                        msgHandler.SendMessage("El servidor se deconectará ");
                        cliente.Shutdown(SocketShutdown.Both);
                        cliente.Disconnect(false);
                    }

                    //cerramos el server
                    socketServer.Shutdown(SocketShutdown.Both);
                    socketServer.Close();
                    Console.WriteLine("Servidor cerrado con exito");
                }
            } catch(Exception e)
            {
                Console.WriteLine("Hubo un error al cerrar el servidor: "+e.Message);
            }
            
        }

        static void ManejarCliente(Socket socketCliente, int nro)
        {
            Console.WriteLine("Cliente {0} conectado", nro);
            MessageCommsHandler msgHandler = new(socketCliente);
            FileCommsHandler fileHandler = new(socketCliente);

            try
            {
                bool clienteConectado = clientesConectados.First(c => c.Key == socketCliente).Value;

                while (clienteConectado)
                {
                    // Leer la selección del cliente
                    string comando = msgHandler.ReceiveMessage();
                    Console.WriteLine("opcion recibida {0}", comando); // debug

                    // Procesar la selección del cliente
                    ProcesarSeleccion(msgHandler, comando, fileHandler);
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

        private static void ProcesarSeleccion(MessageCommsHandler msgHandler, string opcion, FileCommsHandler fileHandler)
        {
            switch (opcion)
            {
                case "0":
                    Console.WriteLine("El cliente entro a la opcion 0"); //debug
                    string datosLogin = msgHandler.ReceiveMessage();
                    Console.WriteLine("Datos de login " + datosLogin); //debug
                    //logica de login
                    msgHandler.SendMessage(""+_userController.VerificarLogin(datosLogin));
                    break;
                case "1":
                    Console.WriteLine("entramos a la opcion 1"); //debug
                    msgHandler.SendMessage(_productController.publicarProducto(msgHandler, fileHandler));
                    break;
                case "2":
                    Console.WriteLine("entramos a la opcion 2"); //debug
                    msgHandler.SendMessage(_userController.agregarProductoACompras(msgHandler));
                    break;
                case "3":
                    Console.WriteLine("entramos a la opcion 3"); //debug
                    msgHandler.SendMessage(_productController.modificarProducto(msgHandler, fileHandler));
                    break;
                case "4":
                    Console.WriteLine("entramos a la opcion 4"); //debug
                    msgHandler.SendMessage(_productController.eliminarProducto(msgHandler));
                    break;
                case "5":
                    Console.WriteLine("entramos a la opcion 5"); //debug
                    msgHandler.SendMessage(_productController.productosBuscados(msgHandler));
                    break;
                case "6":
                    Console.WriteLine("entramos a la opcion 6"); //debug
                    string info = _productController.verMasProducto(msgHandler);
                    string[] datos = info.Split("#");
                    string vaImagen = datos[0];
                    string nombreImagen = datos[1];
                    string mensajeAEnviar = datos[2];
                    msgHandler.SendMessage(mensajeAEnviar);
                    msgHandler.SendMessage(vaImagen);
                    if (vaImagen == "1")
                    {
                        fileHandler.SendFile(filesPath + nombreImagen);
                    }
                    break;
                case "7":
                    string productos = _productController.productosComprados(msgHandler);
                    msgHandler.SendMessage(productos);
                    if(!productos.Contains("El usuario no compró"))
                    {
                        msgHandler.SendMessage(_productController.calificarProducto(msgHandler));
                    }
                    break;
                case "8":
                    Console.WriteLine("entramos a la opcion 8"); //debug
                    msgHandler.SendMessage(_productController.darProductos());
                    break;
                default:
                    // Opción no válida, TODO resolver que hacer
                    break;
            }
        }

        private void agregarUsuarios()
        {
            foreach (string key in usuarios.AllKeys)
            {
                string[] userInfo = usuarios[key].Split(',');
                string correo = userInfo[0];
                string clave = userInfo[1];

             
                _userController.crearUsuario(correo, clave);
            }
        }
        private void agregarProductos() {
            foreach (string key in productos.AllKeys)
            {
                string[] prodInfo = productos[key].Split(',');
                string nombreProd = prodInfo[0];
                string descrProd = prodInfo[1];
                float precio = float.Parse(prodInfo[2]);
                int stock = int.Parse(prodInfo[3]);
                string username = prodInfo[4];

                _productController.agregarProductosBase(nombreProd,descrProd,precio,stock, username);
            }
        }

    }
}

