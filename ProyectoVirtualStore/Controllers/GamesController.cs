using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProyectoVirtualStore.Filters;
using ProyectoVirtualStore.Models;
using ProyectoVirtualStore.Repository;
using System.Security.Claims;

namespace ProyectoVirtualStore.Controllers
{
    public class GamesController : Controller
    {
        private IRepository repo;

        public GamesController(IRepository repo)
        {
            this.repo = repo;
        }

       
        public async Task<IActionResult> Index(int idjuego)
        {
            DatosJuego datosJuego = new DatosJuego();
            datosJuego.Juego = await this.repo.GetJuego(idjuego);
            datosJuego.VistaComentarios = await this.repo.GetVistaComentarios(idjuego);
            return View(datosJuego);
        }


        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> Index(int idjuego,string texto)
        {
            // Obtener la fecha y hora actual como DateTime
            DateTime now = DateTime.Now;

            int idusuario =int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.repo.InsertComentarios(idjuego, idusuario, texto, now);

            DatosJuego datosJuego = new DatosJuego();
            datosJuego.Juego = await this.repo.GetJuego(idjuego);
            datosJuego.VistaComentarios = await this.repo.GetVistaComentarios(idjuego);
            
            return View(datosJuego);
        }

        

    }
}
