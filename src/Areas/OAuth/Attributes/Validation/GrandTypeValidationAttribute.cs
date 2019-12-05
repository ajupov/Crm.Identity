using System.ComponentModel.DataAnnotations;
using Crm.Identity.Areas.OAuth.Models.Types;

namespace Crm.Identity.Areas.OAuth.Attributes.Validation
{
    public class GrandTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var grandType = value?.ToString();
            if (grandType != GrandType.AuthorizationCode &&
                grandType != GrandType.Password &&
                grandType != GrandType.RefreshToken)
            {
                return new ValidationResult("Invalid grand type");
            }

            return ValidationResult.Success;
        }
    }
}