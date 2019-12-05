namespace Crm.Identity.Areas.OAuth.Models.Register
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