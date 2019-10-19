namespace Ajupov.Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse(string callbackUri, bool isInvalidCredentials = false)
        {
            CallbackUri = callbackUri;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string CallbackUri { get; }

        public bool IsInvalidCredentials { get; }
    }
}