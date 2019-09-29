using System;

namespace Ajupov.Identity.Profiles.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public ProfileGender? Gender { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}