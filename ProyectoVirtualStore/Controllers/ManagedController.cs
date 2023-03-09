using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Models;
using ProyectoVirtualStore.Repository;

namespace ProyectoVirtualStore.Controllers
{
    public class ManagedController : Controller
    {

        private IRepository repo;

        public ManagedController(IRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            Usuario user = await this.repo.LogInUser(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {

                HttpContext.Session.SetString("USUARIO", email);
                return RedirectToAction("Index", "Home");
            }


           


        }

        public IActionResult Register() { 
            
            return View();
        }


        [HttpPost]
        public async  Task<IActionResult> Register(string nombreusuario, string password , string email) {

            await this.repo.RegisterUser(nombreusuario,password,email);
            ViewData["MENSAJE"] = "Usuario registrado correctamnet";
            return View();
        }
    }
}
