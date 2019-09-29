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

        [Required]
        public bool IsRemember => RememberState == "on";

        public string RememberState { get; set; }

        [Required]
        [ClientIdValidation]
        public string client_id { get; set; }

        [Required]
        [ResponseTypeValidation]
        public string response_type { get; set; }

        [Required]
        [ScopeValidation]
        public string scope { get; set; }

        [Required]
        [StateValidation]
        public string state { get; set; }

        [RedirectUriWithStateValidation]
        public string redirect_uri { get; set; }
    }
}