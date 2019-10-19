namespace Ajupov.Identity.OAuth.ViewModels
{
    public class ChangeEmailViewModel
    {
        public ChangeEmailViewModel(string oldEmail, string newEmail, bool isInvalidCredentials)
        {
            OldEmail = oldEmail;
            NewEmail = newEmail;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string OldEmail { get; }

        public string NewEmail { get; }

        public bool IsInvalidCredentials { get; }
    }
}