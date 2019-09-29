namespace Ajupov.Identity.OAuth.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel(
            string clientId,
            string responseType,
            string scope,
            string redirectUri,
            string state,
            bool isLoginExists = false,
            bool isEmailExists = false,
            bool isPhoneExists = false)
        {
            ClientId = clientId;
            ResponseType = responseType;
            Scope = scope;
            State = state;
            IsLoginExists = isLoginExists;
            IsEmailExists = isEmailExists;
            IsPhoneExists = isPhoneExists;
            RedirectUri = redirectUri;
        }

        public string ClientId { get; }

        public string ResponseType { get; }

        public string Scope { get; }

        public string State { get; }

        public string RedirectUri { get; }

        public bool IsLoginExists { get; }

        public bool IsEmailExists { get; }

        public bool IsPhoneExists { get; }
    }
}