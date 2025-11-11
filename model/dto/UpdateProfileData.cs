namespace api.agroapp.model.dto
{

    public class UpdateProfileData
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string? telefono { get; set; }

        public UpdateProfileData() { }

        public UpdateProfileData(string nombre, string email, string? telefono)
        {
            this.nombre = nombre;
            this.email = email;
            this.telefono = telefono;
        }
    }

}