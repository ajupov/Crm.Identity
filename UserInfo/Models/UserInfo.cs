using System;

namespace Crm.Identity.UserInfo.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string BirthDate { get; set; }
    }
}