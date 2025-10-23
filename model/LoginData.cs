
namespace api_agroapp.model
{
    public class LoginData
    {
        public string? email { get; set; }
        public string? password { get; set; }

        public LoginData() { }
        public LoginData(string? email, string? password)
        {
            this.email = email;
            this.password = password;
        }

    }
}