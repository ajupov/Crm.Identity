namespace Crm.Identity.OAuth.ViewModels.ChangeEmail
{
    public class ChangeEmailViewModel
    {
        public ChangeEmailViewModel(
            string oldEmail,
            string newEmail,
            bool isEmailNotChanged,
            bool isEmailExists,
            bool isInvalidCredentials)
        {
            OldEmail = oldEmail;
            NewEmail = newEmail;
            IsEmailNotChanged = isEmailNotChanged;
            IsEmailExists = isEmailExists;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string OldEmail { get; }

        public string NewEmail { get; }

        public bool IsEmailNotChanged { get; }

        public bool IsEmailExists { get; }

        public bool IsInvalidCredentials { get; }
    }
}