namespace Ajupov.Identity.src.Identities.Models
{
    public enum IdentityTokenType : byte
    {
        EmailValidation = 1,
        PhoneValidation = 2,
        PasswordReset = 3
    }
}