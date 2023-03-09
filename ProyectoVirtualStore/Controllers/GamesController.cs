using Microsoft.AspNetCore.Mvc;

namespace ProyectoVirtualStore.Controllers
{
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
