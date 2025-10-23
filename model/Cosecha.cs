
using System.ComponentModel.DataAnnotations;

namespace api.agroapp.model
{
    public class Cosecha
    {
        [Key]
        public int id_cosecha { get; set; }
        public int id_lote { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public decimal? rendimiento { get; set; }
        public string? observaciones { get; set; }

        public Cosecha() { }

        public Cosecha(int id_cosecha, int id_lote, DateTime fecha_inicio, DateTime? fecha_fin, decimal? rendimiento, string? observaciones)
        {
            this.id_cosecha = id_cosecha;
            this.id_lote = id_lote;
            this.fecha_inicio = fecha_inicio;
            this.fecha_fin = fecha_fin;
            this.rendimiento = rendimiento;
            this.observaciones = observaciones;
        }

    }
}

