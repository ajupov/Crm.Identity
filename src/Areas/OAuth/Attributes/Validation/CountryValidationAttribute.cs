using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;
using Crm.Identity.Utils.Phone;

namespace Crm.Identity.Areas.OAuth.Attributes.Validation
{
    public class CountryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var countryString = value?.ToString();
            if (countryString.IsEmpty() && countryString.IsValidCountry())
            {
                return new ValidationResult("Invalid country");
            }

            return ValidationResult.Success;
        }
    }
}