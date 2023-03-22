using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{

    [Table("comentarios")]
    public class Comentarios
    {
        [Key]
        [Column("id_comentario")]
        public int IdComentario { get; set; }

        [Column("id_juego")]
        public int IdJuego { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("comentario")]
        public string Comentario { get; set; }

        [Column("fecha_post")]
        public DateTime FechaPost { get; set; }


    }
}
