using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class StateValidationAttribute : ValidationAttribute
    {
        private const int StateLength = 8;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stateString = value?.ToString();
            if (stateString.IsEmpty() || stateString?.Length != StateLength)
            {
                return new ValidationResult("Invalid state value");
            }

            return ValidationResult.Success;
        }
    }
}