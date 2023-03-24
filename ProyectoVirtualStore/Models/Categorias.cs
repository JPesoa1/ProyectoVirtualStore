using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{

    [Table("categorias")]
    public class Categorias
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Column("nombre_categoria")]
        public string NombreCategoria { get; set; }

        [Column("descripcion_categoria")]
        public string? DescripcionCategoria { get; set; }

    }
}
