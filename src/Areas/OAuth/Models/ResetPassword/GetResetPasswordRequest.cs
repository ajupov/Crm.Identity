namespace Crm.Identity.Areas.OAuth.Models.ResetPassword
{
    public class GetResetPasswordRequest
    {
        public string Login { get; set; }

        public bool IsInvalidLogin { get; set; }
    }
}