using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Models.Types;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class GrandTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var grandType = value?.ToString();
            if (grandType != GrandType.AuthorizationCode ||
                grandType != GrandType.Password ||
                grandType != GrandType.RefreshToken)
            {
                return new ValidationResult("Invalid grand type");
            }

            return ValidationResult.Success;
        }
    }
}