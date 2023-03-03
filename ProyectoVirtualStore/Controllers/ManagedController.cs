using Microsoft.AspNetCore.Mvc;

namespace ProyectoVirtualStore.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        { 
            if (email.ToLower() == "admin" && email.ToLower() == "admin")
            {

                HttpContext.Session.SetString("USUARIO", email);
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();

            }


        }
    }
}
