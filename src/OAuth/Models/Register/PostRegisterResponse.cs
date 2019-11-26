namespace Crm.Identity.OAuth.Models.Register
{
    public class PostRegisterResponse
    {
        public PostRegisterResponse(string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public string RedirectUri { get; }
    }
}