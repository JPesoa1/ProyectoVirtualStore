using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{
    [Table("compras")]
    public class Compra
    {
        [Key]
        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("precio_total")]
        public Decimal PrecioTotal{ get; set; }

        [Column("fecha_compra")]
        public DateTime FechaCompra { get; set; }

       
    }
}
