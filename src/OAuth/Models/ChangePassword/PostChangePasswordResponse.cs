namespace Crm.Identity.OAuth.Models.ChangePassword
{
    public class PostChangePasswordResponse
    {
        public PostChangePasswordResponse(bool isInvalidCredentials)
        {
            IsInvalidCredentials = isInvalidCredentials;
        }

        public bool IsInvalidCredentials { get; }
    }
}