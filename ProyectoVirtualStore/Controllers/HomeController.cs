using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Filters;
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




        [AuthorizeUsers]
        public async Task<IActionResult> Index()
        {
            DatosJuegosEstados estados = new DatosJuegosEstados();
            estados.juegosPopular = await this.repo.GetJuegosEstados("popular");
            estados.juegosTendencia = await this.repo.GetJuegosEstados("tendencia");
            estados.juegosEstablecido = await this.repo.GetJuegosEstados("establecido");

           
            return View(estados);
        }


        public async Task<IActionResult> Filtros() 
        {
            List<Juegos> juegos = await this.repo.GetJuegos();
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