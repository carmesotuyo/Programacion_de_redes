using Communication;
using ServerApp.Domain;
using ServerApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Controllers
{
    public class UserController
    {
        private readonly UserLogic _userLogic;
        private readonly ProductLogic _productLogic;
        public UserController()
        {
            _userLogic = new UserLogic();
            _productLogic = new ProductLogic();
        }

        public int VerificarLogin(string userPass)
        {
            return _userLogic.VerificarLogin(userPass);
        }

        public void crearUsuario(string mail, string clave)
        {
            _userLogic.agregarUsuario(mail, clave);
        }

        public string agregarProductoACompras(MessageCommsHandler msgHandler) {
            Console.WriteLine("llegue al metodo");
            string mensajeACliente = "";
            string user = "";
            string nombreProd = "";
            try {
                int i = 1;
                
                StringBuilder retorno = new StringBuilder();
                string informacionRecibida = msgHandler.ReceiveMessage();
                string[] info = informacionRecibida.Split("#");
                user = info[0];
                nombreProd = info[1];
                Usuario u = _userLogic.buscarUsuario(user);

                Producto p = _productLogic.buscarUnProducto(nombreProd);

                _userLogic.agregarProductoACompras(p, u);
                retorno = retorno.AppendLine();
                foreach (Producto prod  in _userLogic.darProductosComprados(u))
                {
                    retorno.AppendLine(i + "- " + prod.Nombre);
                    i++;

                }
                mensajeACliente = "Producto agregado a lista de compras" + retorno.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensajeACliente = "Ocurrió un error: " + ex.Message;

            }
            return mensajeACliente;
        }
    }
}
