namespace Crm.Identity.Password.Settings
{
    public class ResetPasswordSettings
    {
        public string FromAddress { get; set; }

        public string FromName { get; set; }
        
        public string ResetUriPattern { get; set; }
    }
}