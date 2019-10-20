namespace Ajupov.Identity.OAuth.Models.ChangePassword
{
    public class GetChangePasswordRequest
    {
        public string Login { get; set; }

        public bool IsPasswordsNotEqual { get; set; }
        
        public bool IsInvalidCredentials { get; set; }
    }
}