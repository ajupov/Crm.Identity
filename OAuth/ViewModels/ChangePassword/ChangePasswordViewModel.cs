namespace Ajupov.Identity.OAuth.ViewModels.ChangePassword
{
    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel(string login, bool isPasswordsNotEqual, bool isInvalidCredentials)
        {
            Login = login;
            IsPasswordsNotEqual = isPasswordsNotEqual;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string Login { get; }

        public bool IsPasswordsNotEqual { get; }
        
        public bool IsInvalidCredentials { get; }
    }
}