namespace Ajupov.Identity.OAuth.ViewModels
{
    public class ChangePhoneViewModel
    {
        public ChangePhoneViewModel(string oldPhone, string newPhone, bool isInvalidCredentials)
        {
            OldPhone = oldPhone;
            NewPhone = newPhone;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string OldPhone { get; }

        public string NewPhone { get; }

        public bool IsInvalidCredentials { get; }
    }
}