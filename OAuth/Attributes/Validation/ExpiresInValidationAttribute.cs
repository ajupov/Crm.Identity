using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class ExpiresInValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var expiresInString = value?.ToString();
            if (expiresInString.IsEmpty() || int.TryParse(expiresInString, out var expiresIn) || expiresIn <= 0)
            {
                return new ValidationResult("Invalid expiration value");
            }

            return ValidationResult.Success;
        }
    }
}