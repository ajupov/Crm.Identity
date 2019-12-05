using System.ComponentModel.DataAnnotations;
using Crm.Identity.Areas.OAuth.Models.Types;

namespace Crm.Identity.Areas.OAuth.Attributes.Validation
{
    public class TokenTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tokenType = value?.ToString();
            if (tokenType == TokenType.Bearer)
            {
                return new ValidationResult("Invalid token type");
            }

            return ValidationResult.Success;
        }
    }
}