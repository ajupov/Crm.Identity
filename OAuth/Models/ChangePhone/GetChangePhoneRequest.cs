using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ChangePhone
{
    public class GetChangePhoneRequest
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

        public bool IsInvalidCredentials { get; set; }
    }
}