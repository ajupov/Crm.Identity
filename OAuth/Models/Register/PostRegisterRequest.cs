﻿using System.ComponentModel.DataAnnotations;
using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.Register
{
    public class PostRegisterRequest
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Name { get; set; }

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
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

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

        [CountryValidation]
        public string Country { get; set; }
    }
}