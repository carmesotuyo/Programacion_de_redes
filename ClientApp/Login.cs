using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Login
    {
        public static string PedirDatosLogin()
        {
            Console.WriteLine("Seleccionó la opción 0: Iniciar sesión");
            Console.WriteLine("Ingrese su usuario: ");
            string user = Console.ReadLine();
            Console.WriteLine("Ingrese su contraseña: ");
            string pass = Console.ReadLine();
            return user + "#" + pass;
        }
    }
}
