namespace Crm.Identity.OAuth.Models.ChangePhone
{
    public class GetChangePhoneRequest
    {
        public string OldPhone { get; set; }

        public string NewPhone { get; set; }

        public bool IsPhoneNotChanged { get; set; }

        public bool IsPhoneExists { get; set; }

        public bool IsInvalidCredentials { get; set; }
    }
}