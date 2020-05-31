namespace Crm.Identity.OAuth.ViewModels.Register
{
    public class RegisterViewModel
    {
        public RegisterViewModel(
            string clientId,
            string responseType,
            string scope,
            string redirectUri,
            string state,
            string surname,
            string name,
            string login,
            string email,
            string phone,
            bool isPasswordsNotEqual,
            bool isLoginExists,
            bool isEmailExists,
            bool isPhoneExists)
        {
            ClientId = clientId;
            ResponseType = responseType;
            Scope = scope;
            State = state;
            RedirectUri = redirectUri;
            Surname = surname;
            Name = name;
            Login = login;
            Email = email;
            Phone = phone;
            IsPasswordsNotEqual = isPasswordsNotEqual;
            IsLoginExists = isLoginExists;
            IsEmailExists = isEmailExists;
            IsPhoneExists = isPhoneExists;
        }

        public string ClientId { get; }

        public string ResponseType { get; }

        public string Scope { get; }

        public string State { get; }

        public string RedirectUri { get; }

        public string Surname { get; }

        public string Name { get; }

        public string Login { get; }

        public string Email { get; }

        public string Phone { get; }

        public bool IsPasswordsNotEqual { get; }

        public bool IsLoginExists { get; }

        public bool IsEmailExists { get; }

        public bool IsPhoneExists { get; }
    }
}
