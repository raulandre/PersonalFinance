namespace PersonalFinance.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public LoginViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
