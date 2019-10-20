namespace Ajupov.Identity.src.OAuth.ViewModels.ResetPassword
{
    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel(string login, bool isInvalidLogin)
        {
            Login = login;
            IsInvalidLogin = isInvalidLogin;
        }

        public string Login { get; }

        public bool IsInvalidLogin { get; }
    }
}