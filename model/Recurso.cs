
namespace api.agroapp.model
{
    public class Recurso
    {
        public int id_recurso { get; set; }
        public string? nombre { get; set; }
        public string? tipo { get; set; }
        public string? marca { get; set; }
        public string? modelo { get; set; }
        public string? estado { get; set; }
        public Recurso() { }
        public Recurso(int id_recurso, string? nombre, string? tipo, string? marca, string? modelo, string? estado)
        {
            this.id_recurso = id_recurso;
            this.nombre = nombre;
            this.tipo = tipo;
            this.marca = marca;
            this.modelo = modelo;
            this.estado = estado;
        }

    }
}

