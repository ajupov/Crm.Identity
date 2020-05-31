using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.Register
{
    public class GetRegisterRequest
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

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsPasswordsNotEqual { get; set; }

        public bool IsLoginExists { get; set; }

        public bool IsEmailExists { get; set; }

        public bool IsPhoneExists { get; set; }
    }
}
