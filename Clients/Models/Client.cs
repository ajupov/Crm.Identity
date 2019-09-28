using System;
using System.Collections.Generic;

namespace Crm.Identity.Clients.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUriPattern { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public List<ClientScope> Scopes { get; set; }
    }
}