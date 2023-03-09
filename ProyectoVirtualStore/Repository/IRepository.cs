using ProyectoVirtualStore.Models;

namespace ProyectoVirtualStore.Repository
{
    public interface IRepository
    {
        List<Juegos> GetJuegos();

        Task RegisterUser(string nombreusuario, string password, string email);

        Task<Usuario> LogInUser(string email, string password);

    }
}
