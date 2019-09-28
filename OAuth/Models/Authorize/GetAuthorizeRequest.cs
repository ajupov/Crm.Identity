using System.ComponentModel.DataAnnotations;
using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.Authorize
{
    public class GetAuthorizeRequest
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
    }
}