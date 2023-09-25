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
        public UserController()
        {
            _userLogic = new UserLogic();
        }

        public int VerificarLogin(string userPass)
        {
            return _userLogic.VerificarLogin(userPass);
        }

        public string Login(bool autenticado)
        {
            string respuesta = "Usuario o contraseña incorrecta";
            if (autenticado)
            {
                respuesta = "Login exitoso";
            }
            return respuesta;
        }

        public void crearUsuario(string mail, string clave)
        {
            _userLogic.agregarUsuario(mail, clave);
        }
    }
}
