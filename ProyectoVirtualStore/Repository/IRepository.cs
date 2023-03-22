using ProyectoVirtualStore.Models;

namespace ProyectoVirtualStore.Repository
{
    public interface IRepository
    {
        Task<List<Juegos>> GetJuegos();

        Task RegisterUser(string nombreusuario, string password, string email);

        Task<Usuario> LogInUser(string email, string password);

        Task<Juegos> GetJuego(int id);


        Task<List<Comentarios>> GetComentarios(int id);
        Task InsertComentarios(int idjuego,int idusuario,string comentario, DateTime fecha);

    }
}
