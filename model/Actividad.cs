

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_agroapp.model
{
    public class Actividad
    {
        [Key]
        [Column("id_actividad")]
        public int IdActividad { get; set; }

        [Column("id_lote")]
        public int Id_lote { get; set; }

        [Column("id_insumo")]
        public int? id_insumo { get; set; }

        [Column("id_recurso")]
        public int? id_recurso { get; set; }

        [Column("id_tipo_actividad")]
        public int id_tipo_actividad { get; set; }
        [Column("descripcion")]
        public string descripcion { get; set; }
        [Column("fecha_inicio")]
        public DateTime fecha_inicio { get; set; }
        [Column("fecha_fin")]
        public DateTime? fecha_fin { get; set; }
        [Column("cantidad_insumo")]
        public decimal? cantidad_insumo { get; set; }
        [Column("costo")]
        public decimal? costo { get; set; }


        public Actividad() { }

        public Actividad(int Id_lote, int? id_insumo, int? id_recurso, int id_tipo_actividad, string descripcion, DateTime fecha_inicio, DateTime? fecha_fin, decimal? cantidad_insumo, decimal? costo)
        {

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
