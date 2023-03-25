using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{

    [Table("imagenes")]
    public class Imagenes
    {
        [Key]
        [Column("id_imagen")]
        public int IdImagen { get; set; }


        [Column("id_juego")]
        public int IdJuego { get; set; }


        [Column("imagen")]
        public string Imagen { get; set; }

    }
}
