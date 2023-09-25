using ServerApp.Database;
using ServerApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Logic
{
    public class UserLogic
    {
        private readonly SingletonDB _database;
        public UserLogic()
        {
            _database = SingletonDB.GetInstance();
        }
        public int VerificarLogin(string userPass)
        {
            int autenticado = 0;
            string[] userPassArray = userPass.Split('#');
            string user = userPassArray[0];
            string pass = userPassArray[1];
            List<Usuario> usuarios = _database.usuarios();

            Usuario usuario = usuarios.FirstOrDefault(u => u.mail == user);

            
            if (usuario != null && usuario.clave == pass)
            {
                autenticado = 1;
            }

            return autenticado;
        }

        //metodo para agregar un usuario
        public List<Usuario> agregarUsuario(string mail, string clave)
        {
            Usuario newUser = new Usuario(mail, clave);
            _database.agregarUsuario(newUser);
            return _database.usuarios();
        }

    }
}
