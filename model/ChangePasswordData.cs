
namespace api.agroapp.model
{
    public class ChangePasswordData
    {
        public int id_usuario { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}