
namespace api.agroapp.model
{
    public class CampoData
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public decimal extension_ha { get; set; }
        public decimal? longitud { get; set; }
        public decimal? latitud { get; set; }

        public CampoData() { }

        public CampoData(int id_usuario, string nombre, string ubicacion, decimal extension_ha, decimal? longitud = null, decimal? latitud = null)
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