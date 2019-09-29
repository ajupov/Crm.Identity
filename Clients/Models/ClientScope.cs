using System;

namespace Ajupov.Identity.Clients.Models
{
    public class ClientScope
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public string Value { get; set; }
    }
}