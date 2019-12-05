using System;

namespace Crm.Identity.Areas.Identities.Models
{
    public class Identity
    {
        public Guid Id { get; set; }

        public Guid ProfileId { get; set; }

        public IdentityType Type { get; set; }

        public string Key { get; set; }

        public string PasswordHash { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}