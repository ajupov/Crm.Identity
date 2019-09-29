using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class ClientSecretValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid client secret");
            }

            return ValidationResult.Success;
        }
    }
}