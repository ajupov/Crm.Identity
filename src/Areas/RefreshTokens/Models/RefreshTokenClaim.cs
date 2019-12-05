using System;

namespace Crm.Identity.Areas.RefreshTokens.Models
{
    public class RefreshTokenClaim
    {
        public Guid Id { get; set; }

        public Guid RefreshTokenId { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}