using Ajupov.Identity.src.OAuth.Attributes.Validation;

namespace Ajupov.Identity.src.OAuth.Models.Authorize
{
    public class GetAuthorizeRequest
    {
        [ClientIdValidation]
        public string client_id { get; set; }

        [ResponseTypeValidation]
        public string response_type { get; set; }

        [ScopeValidation]
        public string scope { get; set; }

        [StateValidation]
        public string state { get; set; }

        [RedirectUriValidation]
        public string redirect_uri { get; set; }

        public bool IsInvalidCredentials { get; set; }
    }
}