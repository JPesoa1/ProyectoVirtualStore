using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoVirtualStore.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario{ get; set; }

        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }

        [Column("correo_electronico")]
        public string Email { get; set; }

        

        [Column("pass")]
        public byte[] Password { get; set; }

        [Column("imagen")]
        public string? Imagen { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

      

       

    }
}
