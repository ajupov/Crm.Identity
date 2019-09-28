using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class RedirectUriValidationAttribute : ValidationAttribute
    {
        private const int MaxUriLength = 2048;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var redirectUri = value?.ToString();
            if (redirectUri.IsEmpty() || redirectUri?.Length > MaxUriLength)
            {
                return new ValidationResult("Invalid redirect uri");
            }

            return ValidationResult.Success;
        }
    }
}