using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{
    [Table("V_COMENTARIO_USUARIO")]
    public class VistaComentarios
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }




        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }


        [Column("imagen")]
        public string? Imange { get; set; }

        [Column("id_juego")]
        public int IdJuego { get; set; }


        [Column("comentario")]
        public string Comentario { get; set; }


        [Column("fecha_post")]
        public DateTime FechaPost { get; set; }


    }
}
