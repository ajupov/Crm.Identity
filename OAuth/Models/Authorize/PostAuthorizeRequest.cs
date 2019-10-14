using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Attributes.Validation;

namespace Ajupov.Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeRequest
    {
        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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
    }
}