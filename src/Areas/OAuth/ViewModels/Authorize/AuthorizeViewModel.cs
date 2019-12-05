namespace Crm.Identity.Areas.OAuth.ViewModels.Authorize
{
    public class AuthorizeViewModel
    {
        public AuthorizeViewModel(
            string clientId,
            string responseType,
            string scope,
            string redirectUri,
            string state,
            bool isInvalidCredentials)
        {
            ClientId = clientId;
            ResponseType = responseType;
            Scope = scope;
            State = state;
            RedirectUri = redirectUri;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string ClientId { get; }

        public string ResponseType { get; }

        public string Scope { get; }

        public string State { get; }

        public string RedirectUri { get; }

        public bool IsInvalidCredentials { get; }
    }
}