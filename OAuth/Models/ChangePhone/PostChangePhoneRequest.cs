using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ChangePhone
{
    public class PostChangePhoneRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        [Phone]
        public string OldPhone { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        [Phone]
        public string NewPhone { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}