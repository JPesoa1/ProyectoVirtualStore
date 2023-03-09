using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Data;
using ProyectoVirtualStore.Helpers;
using ProyectoVirtualStore.Models;

namespace ProyectoVirtualStore.Repository
{
    public class RepositorySQLTienda : IRepository
    {
        private TiendaContext context;

        public RepositorySQLTienda(TiendaContext context)
        {
            this.context = context;
        }

        public List<Juegos> GetJuegos()
        {
            var consulta = from datos in this.context.Juegos
                           select datos;
            return consulta.ToList();
        }


        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUsuario) + 1;
            }

        }

        public async Task RegisterUser(string nombreusuario, string password, string email)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = this.GetMaxIdUsuario();
            usuario.NombreUsuario = nombreusuario;
            usuario.Email = email;
            usuario.Salt = HelperCryptography.GenerateSalt();
            usuario.Password = HelperCryptography.EncryptPassword(password, usuario.Salt);

            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUser(string email, string password)
        {
            List<Usuario> lista = await this.context.Usuarios.ToListAsync();
            Usuario user =this.context.Usuarios.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                
                byte[] passUsuario = user.Password;
                
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                
                bool respuesta =
                    HelperCryptography.CompareArrays(passUsuario, temp);
                if (respuesta == true)
                {
                    //SON IGUALES
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
