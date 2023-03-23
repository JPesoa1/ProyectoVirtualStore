using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Data;
using ProyectoVirtualStore.Helpers;
using ProyectoVirtualStore.Models;
using System.Diagnostics.Metrics;

#region

//ALTER VIEW V_COMENTARIO_USUARIO
//AS
//	SELECT	u.id_usuario, u.nombre_usuario , u.imagen, c.id_juego , c.comentario , c.fecha_post
//	FROM USUARIOS u
//	INNER JOIN COMENTARIOS c ON u.id_usuario=c.id_usuario;
//GO
#endregion





namespace ProyectoVirtualStore.Repository
{
    public class RepositorySQLTienda : IRepository
    {
        private TiendaContext context;

        public RepositorySQLTienda(TiendaContext context)
        {
            this.context = context;
        }

        public async Task<List<Juegos>> GetJuegos()
        {
            var consulta = from datos in this.context.Juegos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Juegos>> GetJuegosEstados(string estado)
        {
            var consulta = from datos in this.context.Juegos
                           where datos.Estado ==estado
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Juegos> GetJuego(int id)
        {
            return await this.context.Juegos.FirstOrDefaultAsync(x => x.IdJuego == id);
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
            Usuario user = this.context.Usuarios.FirstOrDefault(x => x.Email == email);
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

        public async Task<List<Comentarios>> GetComentarios(int idjuego)
        {
            return await this.context.Comentarios.Where(x => x.IdJuego == idjuego).ToListAsync();
        }


        public async Task<List<VistaComentarios>> GetVistaComentarios(int id)
        {

            return await this.context.VistaComentarios.Where(x => x.IdJuego == id).ToListAsync();


        }

        private int MaxIdComentario()
        {

            if (this.context.Comentarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Comentarios.Max(z => z.IdComentario) + 1;
            }

        }

        public async Task InsertComentarios(int idjuego, int idusuario, string comentario, DateTime fecha)
        {
            Comentarios comentarios = new Comentarios();
            comentarios.Comentario = comentario;
            comentarios.IdUsuario = idusuario;
            comentarios.IdJuego = idjuego;
            comentarios.IdComentario = this.MaxIdComentario();
            comentarios.FechaPost = fecha;

            this.context.Comentarios.Add(comentarios);
            //GUARDAMOS CAMBIOS EN LA BASE DE DATOS
            await this.context.SaveChangesAsync();
        }


    }
}
