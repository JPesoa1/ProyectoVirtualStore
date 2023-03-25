using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoVirtualStore.Models
{
    [Table("compra_juego")]
    public class CompraJuego
    {
        [Key]
        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Key]   
        [Column("id_juego")]
        public int IdJuego { get; set; }

        [Column("precio_juego")]
        public Decimal PrecioJuego { get; set; }


    }
}
