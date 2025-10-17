
namespace api.agroapp.model
{
    public class TipoActividad
    {
        public int id_tipo_actividad { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public TipoActividad() { }
        public TipoActividad(int id_tipo_actividad, string? nombre, string? descripcion)
        {
            this.id_tipo_actividad = id_tipo_actividad;
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

    }


}

