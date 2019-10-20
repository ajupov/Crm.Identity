using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.src.OAuth.Models.Types;

namespace Ajupov.Identity.src.OAuth.Attributes.Validation
{
    public class ResponseTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var responseType = value?.ToString();
            if (responseType != ResponseType.Code && responseType != ResponseType.Token)
            {
                return new ValidationResult("Invalid response type");
            }

            return ValidationResult.Success;
        }
    }
}