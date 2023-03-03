using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{
    [Table("Juegos")]
    public class Juegos
    {
        [Key]
        [Column("idJuego")]
        public int IdJuego { get; set; }


        [Column("nombreJuego")]
        public string NombreJuego { get; set; }


        [Column("descripcionJuego")]
        public string DescripcionJuego { get; set; }

        [Column("precioJuego")]
        public Decimal PrecioJuego { get; set; }


        [Column("fechaLanzamiento")]
        public DateTime FechaLanzamiento { get; set; }


        [Column("categoriaJuego")]
        public string CategoriaJuego { get; set; }


        [Column("modalidad")]
        public string? Modalidad { get; set; }



    }
}
