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
        private IWebHostEnvironment webHostEnvironment;
        private IRepository repo;

        public ManagedController(IWebHostEnvironment webHostEnvironment, IRepository repo)
        {
            this.webHostEnvironment = webHostEnvironment;
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
            return RedirectToAction("Index","Home");
        }



        [AuthorizeUsers]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }


        [AuthorizeUsers]
        public async Task<IActionResult> User() 
        {
            string nombre =HttpContext.Session.GetString("USUARIO");
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Usuario usuario = await this.repo.FindUsuario(idusuario);
            return View(usuario);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async  Task<IActionResult> User(IFormFile file)
        {
            string rooFolder = this.webHostEnvironment.WebRootPath;
           

            string filename =  file.FileName;
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string path = Path.Combine(rooFolder,"perfil",filename);
            string filenameUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value + "_" + filename;
            string newpath = Path.Combine(rooFolder, "perfil", filenameUsuario);

            using (Stream stream = new FileStream(path, FileMode.Create))
            { 
                await file.CopyToAsync(stream);
            }
            if (System.IO.File.Exists(path))
            {
                // Cambiar el nombre del archivo
                System.IO.File.Move(path, newpath);

                // Actualizar la base de datos u otros datos relevantes con el nuevo nombre de archivo
                await this.repo.ModificarUsuarioImagen(idusuario, filenameUsuario);
            }
           

            using (Stream stream = new FileStream(newpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await this.repo.ModificarUsuarioImagen(idusuario,filenameUsuario);
            string nombre = HttpContext.Session.GetString("USUARIO");

            Usuario usuario = await this.repo.FindUsuario(idusuario);
            return View(usuario);
        }

    }
}
