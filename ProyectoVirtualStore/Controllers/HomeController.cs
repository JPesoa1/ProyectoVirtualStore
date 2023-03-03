using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Models;
using ProyectoVirtualStore.Repository;
using System.Diagnostics;

namespace ProyectoVirtualStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository repo;

        public HomeController(ILogger<HomeController> logger, IRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Juegos> juegos = this.repo.GetJuegos();
            return View(juegos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}