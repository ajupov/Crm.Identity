using System.ComponentModel.DataAnnotations;
using Crm.Identity.Areas.OAuth.Models.Types;

namespace Crm.Identity.Areas.OAuth.Attributes.Validation
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