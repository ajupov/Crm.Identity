using System.ComponentModel.DataAnnotations;
using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.Register
{
    public class PostRegisterRequest
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
        [StringLength(256)]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

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