namespace Crm.Identity.OAuth.ViewModels.ChangePhone
{
    public class ChangePhoneViewModel
    {
        public ChangePhoneViewModel(
            string oldPhone,
            string newPhone,
            bool isPhoneNotChanged,
            bool isPhoneExists,
            bool isInvalidCredentials)
        {
            OldPhone = oldPhone;
            NewPhone = newPhone;
            IsPhoneNotChanged = isPhoneNotChanged;
            IsPhoneExists = isPhoneExists;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string OldPhone { get; }

        public string NewPhone { get; }

        public bool IsPhoneNotChanged { get; }

        public bool IsPhoneExists { get; }

        public bool IsInvalidCredentials { get; }
    }
}