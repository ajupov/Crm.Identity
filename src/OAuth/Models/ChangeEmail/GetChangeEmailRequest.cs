namespace Ajupov.Identity.src.OAuth.Models.ChangeEmail
{
    public class GetChangeEmailRequest
    {
        public string OldEmail { get; set; }

        public string NewEmail { get; set; }

        public bool IsEmailNotChanged { get; set; }

        public bool IsEmailExists { get; set; }

        public bool IsInvalidCredentials { get; set; }
    }
}