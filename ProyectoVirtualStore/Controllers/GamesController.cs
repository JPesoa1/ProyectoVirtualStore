using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Extensions;
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

        [AuthorizeUsers]
        public async Task<IActionResult> Index(int idjuego, int? idjuegocarrito)
        {
            if (idjuegocarrito != null) 
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");

                }
                if (carrito.Contains(idjuegocarrito.Value) == false) {
                    carrito.Add(idjuegocarrito.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                
                }

            }
            ViewData["IMAGENES"] = await this.repo.GetImagenes(idjuego);

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

            ViewData["IMAGENES"] = await this.repo.GetImagenes(idjuego);

            datosJuego.Juego = await this.repo.GetJuego(idjuego);
            datosJuego.VistaComentarios = await this.repo.GetVistaComentarios(idjuego);
            
            return View(datosJuego);
        }

        [AuthorizeUsers]
        public async  Task<IActionResult> Carrito(int? idproducto)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito == null)
            {
                return View();
            }
            else 
            {
                if (idproducto != null) { 
                    carrito.Remove(idproducto.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);

                }
                List<Juegos> juegos = await this.repo.GetJuegosCarritosAsync(carrito);
                return View(juegos);
            }

        }

        public async Task<IActionResult> Compra() {

            DateTime now = DateTime.Now;


            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);


            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Juegos> juegos = await this.repo.GetJuegosCarritosAsync(carrito);


            await this.repo.InsertarCompra(juegos,idusuario,now);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("Index","Home");

        }

        


        

    }
}
