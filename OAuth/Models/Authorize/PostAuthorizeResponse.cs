namespace Crm.Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse()
        {
        }

        public PostAuthorizeResponse(string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public static PostAuthorizeResponse ErrorResponse = new PostAuthorizeResponse
        {
            HasError = true
        };

        public bool HasError { get; set; }

        public string RedirectUri { get; set; }
    }
}