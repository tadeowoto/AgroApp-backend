
namespace api.agroapp.model
{
    public class Lote
    {
        public int id_lote { get; set; }
        public int id_campo { get; set; }
        public string? nombre { get; set; }
        public decimal? superficie_ha { get; set; }
        public string? cultivo { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public Lote() { }
        public Lote(int id_lote, int id_campo, string? nombre, decimal? superficie_ha, string? cultivo, DateTime? fecha_creacion)
        {
            this.id_lote = id_lote;
            this.id_campo = id_campo;
            this.nombre = nombre;
            this.superficie_ha = superficie_ha;
            this.cultivo = cultivo;
            this.fecha_creacion = fecha_creacion;
        }

    }
}

