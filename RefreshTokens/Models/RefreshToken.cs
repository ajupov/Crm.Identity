using System;
using System.Collections.Generic;

namespace Crm.Identity.RefreshTokens.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid ProfileId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public List<RefreshTokenClaim> Claims { get; set; }
    }
}
