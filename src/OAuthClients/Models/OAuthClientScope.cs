using System;

namespace Crm.Identity.OAuthClients.Models
{
    public class OAuthClientScope
    {
        public Guid Id { get; set; }

        public Guid OAuthClientId { get; set; }

        public string Value { get; set; }
    }
}