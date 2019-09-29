using System;
using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Attributes.Validation;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.OAuth.Models.Register
{
    public class PostRegisterRequest
    {
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Name { get; set; }

        public ProfileGender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        [Phone]
        public string Phone { get; set; }

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

        [RedirectUriValidation]
        public string redirect_uri { get; set; }
    }
}