using System.Collections.Generic;
using System.Linq;

namespace Crm.Identity.Registration.Models
{
    public class RegisterResponseModel
    {
        public RegisterResponseModel(IEnumerable<string> errors)
        {
            Errors = errors.ToList();
        }

        public List<string> Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();
    }
}