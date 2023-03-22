using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Filters;
using ProyectoVirtualStore.Models;
using ProyectoVirtualStore.Repository;

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
            datosJuego.Comentarios = await this.repo.GetComentarios(idjuego);
            return View(datosJuego);
        }


        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> Index(int idjuego,string texto)
        {
            // Obtener la fecha y hora actual como DateTime
            DateTime now = DateTime.Now;

            await this.repo.InsertComentarios(idjuego, 5, texto, now);

            DatosJuego datosJuego = new DatosJuego();
            datosJuego.Juego = await this.repo.GetJuego(idjuego);
            datosJuego.Comentarios = await this.repo.GetComentarios(idjuego);
            
            return View(datosJuego);
        }

    }
}
