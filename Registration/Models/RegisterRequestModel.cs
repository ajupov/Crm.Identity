using System.ComponentModel.DataAnnotations;
using Crm.Identity.Profiles.Models;

namespace Crm.Identity.Registration.Models
{
    public class RegisterRequestModel
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
        public ProfileGender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(256)]
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
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}