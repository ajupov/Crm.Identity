namespace Ajupov.Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse(string redirectUri, bool isInvalidCredentials = false)
        {
            RedirectUri = redirectUri;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string RedirectUri { get; }
        
        public bool IsInvalidCredentials { get; }
    }
}