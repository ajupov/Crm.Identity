using System;
using System.Collections.Generic;

namespace Ajupov.Identity.OAuthClients.Models
{
    public class OAuthClient
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUriPattern { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public List<OAuthClientScope> Scopes { get; set; }
    }
}