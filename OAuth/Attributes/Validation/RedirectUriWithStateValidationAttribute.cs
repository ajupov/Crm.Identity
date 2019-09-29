using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class RedirectUriWithStateValidationAttribute : RedirectUriValidationAttribute
    {
        private const int StateLength = 8;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var baseValidationResult = base.IsValid(value, validationContext);
            if (baseValidationResult != ValidationResult.Success)
            {
                return baseValidationResult;
            }

            var redirectUri = value?.ToString();
            var state = new Uri(redirectUri).Query.Split('&', '?').FirstOrDefault(x => x.Contains("state"));
            if (state.IsEmpty() || state?.Length != StateLength)
            {
                return new ValidationResult("Invalid state value");
            }

            return ValidationResult.Success;
        }
    }
}