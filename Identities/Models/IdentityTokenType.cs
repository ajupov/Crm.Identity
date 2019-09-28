namespace Crm.Identity.Identities.Models
{
    public enum IdentityTokenType : byte
    {
        EmailValidation = 1,
        PhoneValidation = 2,
        AccessToken = 3,
        RefreshToken = 4
    }
}