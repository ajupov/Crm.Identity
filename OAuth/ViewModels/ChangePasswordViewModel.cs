namespace Ajupov.Identity.OAuth.ViewModels
{
    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel(string login, bool isInvalidCredentials)
        {
            Login = login;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string Login { get; }

        public bool IsInvalidCredentials { get; }
    }
}