
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_agroapp.model
{
    public class Campo
    {
        [Key]
        public int id_campo { get; set; }
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public decimal extension_ha { get; set; }
        [Column(TypeName = "decimal(11,8)")]
        public decimal? longitud { get; set; }
        [Column(TypeName = "decimal(11,8)")]
        public decimal? latitud { get; set; }

        public Campo() { }
        public Campo(int id_campo, int id_usuario, string nombre, string ubicacion, decimal extension_ha, decimal? longitud, decimal? latitud)
        {
            this.id_campo = id_campo;
            this.id_usuario = id_usuario;
            this.nombre = nombre;
            this.ubicacion = ubicacion;
            this.extension_ha = extension_ha;
            this.longitud = longitud;
            this.latitud = latitud;
        }

        public Campo(int id_usuario, string nombre, string ubicacion, decimal extension_ha, decimal? longitud = null, decimal? latitud = null)
        {
            this.id_usuario = id_usuario;
            this.nombre = nombre;
            this.ubicacion = ubicacion;
            this.extension_ha = extension_ha;
            this.longitud = longitud;
            this.latitud = latitud;
        }


    }
}

