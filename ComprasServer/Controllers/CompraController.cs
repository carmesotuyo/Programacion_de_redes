using Microsoft.AspNetCore.Mvc;

namespace ComprasServer.Controllers
{
    public class CompraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
