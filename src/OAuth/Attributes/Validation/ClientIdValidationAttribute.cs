using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.src.OAuth.Attributes.Validation
{
    public class ClientIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var clientIdString = value?.ToString();
            if (clientIdString.IsEmpty())
            {
                return new ValidationResult("Invalid client_id");
            }

            return ValidationResult.Success;
        }
    }
}