using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{
    [Table("juegos")]
    public class Juegos
    {
        [Key]
        [Column("id_juego")]
        public int IdJuego { get; set; }


        [Column("nombre_juego")]
        public string NombreJuego { get; set; }


        [Column("descripcion_juego")]
        public string DescripcionJuego { get; set; }

        [Column("precio_juego")]
        public Decimal PrecioJuego { get; set; }

        [Column("estado")]
        public string Estado{ get; set; }


        [Column("imagen")]
        public string Imagen { get; set; }



    }
}
