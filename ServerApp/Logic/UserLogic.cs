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
        public Usuario agregarUsuario(string mail, string clave)
        {
            Usuario newUser = new Usuario(mail, clave);
            validarUsuarioRepetido(newUser);
            _database.agregarUsuario(newUser);
            return newUser;
        }

        private void validarUsuarioRepetido(Usuario usuario)
        {
            if (_database.existeUsuario(usuario))
            {
                throw new Exception("El mail que intentas ingresar ya está en uso, prueba con otro");
            }
        }
        public List<Producto> darProductosComprados(Usuario u)
        {
            return _database.darListaProductosCompradosPorUsuario(u);
        }
        public List<Producto> agregarProductoACompras(Producto p, Usuario u)
        {
            if (_database.existeProducto(p))
            {
                if (_database.tieneStock(p))
                {
                    _database.agregarProductoACompras(p, u);
                    return u.comprados;
                }
                else
                {
                    throw new Exception("El producto que quieres comprar no tiene stock disponible");
                }

            }
            else
            {
                throw new Exception("El producto que quieres comprar no existe");
            }
        }
        public Usuario buscarUsuario(string username)
        {
            return _database.buscarUsuario(username);
        }

    }
}

