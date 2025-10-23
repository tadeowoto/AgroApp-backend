
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.agroapp.model
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string? telefono { get; set; }
        public DateTime fecha_registro { get; set; }

        public Usuario() { }

        public Usuario(int id_usuario, string nombre, string email, string password, string? telefono, DateTime fecha_registro)
        {
            this.id_usuario = id_usuario;
            this.nombre = nombre;
            this.email = email;
            this.password = password;
            this.telefono = telefono;
            this.fecha_registro = fecha_registro;
        }

    }
}

