using System;

namespace Ajupov.Identity.src.Resources.Models
{
    public class Resource
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Scope { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}