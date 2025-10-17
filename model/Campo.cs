
namespace api_agroapp.model
{
    public class Campo
    {
        public int id_campo { get; set; }
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public decimal extension_ha { get; set; }

        public Campo() { }
        public Campo(int id_campo, int id_usuario, string nombre, string ubicacion, decimal extension_ha)
        {
            this.id_campo = id_campo;
            this.id_usuario = id_usuario;
            this.nombre = nombre;
            this.ubicacion = ubicacion;
            this.extension_ha = extension_ha;
        }

    }
}

