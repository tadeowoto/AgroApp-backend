

namespace api_agroapp.model
{
    public class Actividad
    {
        public int IdActividad { get; set; }
        public int Id_lote { get; set; }
        public int? id_insumo { get; set; }
        public int? id_recurso { get; set; }
        public int id_tipo_actividad { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public decimal? cantidad_insumo { get; set; }
        public decimal? costo { get; set; }


        public Actividad() { }

        public Actividad(int IdActividad, int Id_lote, int? id_insumo, int? id_recurso, int id_tipo_actividad, string descripcion, DateTime fecha_inicio, DateTime? fecha_fin, decimal? cantidad_insumo, decimal? costo)
        {
            this.IdActividad = IdActividad;
            this.Id_lote = Id_lote;
            this.id_insumo = id_insumo;
            this.id_recurso = id_recurso;
            this.id_tipo_actividad = id_tipo_actividad;
            this.descripcion = descripcion;
            this.fecha_inicio = fecha_inicio;
            this.fecha_fin = fecha_fin;
            this.cantidad_insumo = cantidad_insumo;
            this.costo = costo;
        }



    }


}
