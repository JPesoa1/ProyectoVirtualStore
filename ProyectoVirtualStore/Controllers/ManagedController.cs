using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using ProyectoVirtualStore.Filters;
using ProyectoVirtualStore.Models;
using ProyectoVirtualStore.Repository;

using System.Security.Claims;

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
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.NameIdentifier);

                Claim claimId = new Claim(ClaimTypes.NameIdentifier,user.IdUsuario.ToString());
                Claim claimUser = new Claim(ClaimTypes.Name, user.NombreUsuario);
                Claim claimEmail = new Claim(ClaimTypes.Email, user.Email);



                identity.AddClaim(claimId);
                identity.AddClaim(claimUser);
                identity.AddClaim(claimEmail);
             




                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);


                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                string idjuego = TempData["idjuego"].ToString();

                return RedirectToAction(action, controller , new {idjuego = idjuego});
               
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



        [AuthorizeUsers]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult User() 
        {
            string nombre =HttpContext.Session.GetString("USUARIO");
            return View(nombre);
        }

    }
}
