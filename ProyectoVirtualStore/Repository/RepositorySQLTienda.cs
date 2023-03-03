using ProyectoVirtualStore.Data;
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
            return this.context.Juegos.ToList<Juegos>();
        }
    }
}
