using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.Types;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class RedirectUriValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return validationContext.ObjectInstance switch
            {
                GetAuthorizeRequest _ when value?.ToString().IsEmpty() == true
                => new ValidationResult("Invalid redirect uri"),
                TokenRequest tokenRequest when value?.ToString().IsEmpty() == true &&
                                               tokenRequest.grant_type == GrandType.AuthorizationCode
                => new ValidationResult("Invalid redirect uri"),
                _ => ValidationResult.Success
            };
        }
    }
}