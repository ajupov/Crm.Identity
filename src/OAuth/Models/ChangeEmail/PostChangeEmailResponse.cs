namespace Ajupov.Identity.src.OAuth.Models.ChangeEmail
{
    public class PostChangeEmailResponse
    {
        public PostChangeEmailResponse(bool isInvalidCredentials)
        {
            IsInvalidCredentials = isInvalidCredentials;
        }

        public bool IsInvalidCredentials { get; }
    }
}