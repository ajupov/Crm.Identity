namespace Ajupov.Identity.OAuth.Models.Register
{
    public class PostRegisterResponse
    {
        public PostRegisterResponse()
        {
        }

        public PostRegisterResponse(string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public static PostRegisterResponse ErrorResponse = new PostRegisterResponse
        {
            HasError = true
        };

        public bool HasError { get; set; }

        public string RedirectUri { get; set; }
    }
}