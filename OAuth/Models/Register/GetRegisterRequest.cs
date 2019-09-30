using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Attributes.Validation;

namespace Ajupov.Identity.OAuth.Models.Register
{
    public class GetRegisterRequest
    {
        [Required]
        [ClientIdValidation]
        public string client_id { get; set; }

        [Required]
        [ResponseTypeValidation]
        public string response_type { get; set; }

        [Required]
        [ScopeValidation]
        public string scope { get; set; }

        [StateValidation]
        public string state { get; set; }

        [RedirectUriValidation]
        public string redirect_uri { get; set; }

        public bool IsLoginExists { get; set; }
        
        public bool IsEmailExists { get; set; }
        
        public bool IsPhoneExists { get; set; }
    }
}