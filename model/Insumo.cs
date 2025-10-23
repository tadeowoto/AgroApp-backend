
using System.ComponentModel.DataAnnotations;

namespace api_agroapp.model
{

    public class Insumo
    {

        [Key]
        public int id_insumo { get; set; }
        public string? nombre { get; set; }
        public string? tipo { get; set; }
        public string? unidad { get; set; }
        public decimal? stock_actual { get; set; }
        public DateTime? fecha_vencimiento { get; set; }

        public Insumo() { }

        public Insumo(int id_insumo, string? nombre, string? tipo, string? unidad, decimal? stock_actual, DateTime? fecha_vencimiento)
        {
            this.id_insumo = id_insumo;
            this.nombre = nombre;
            this.tipo = tipo;
            this.unidad = unidad;
            this.stock_actual = stock_actual;
            this.fecha_vencimiento = fecha_vencimiento;
        }

    }

}
