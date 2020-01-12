using System;

namespace Crm.Identity.OAuth.Models.UserInfo
{
    public class UserInfoResponse
    {
        public UserInfoResponse(Guid id, string email, string phone, string surname, string name)
        {
            this.id = id;
            this.email = email;
            this.phone = phone;
            this.surname = surname;
            this.name = name;
        }

        public UserInfoResponse(string error)
        {
            this.error = error;
        }

        public Guid id { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string surname { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public string birth_date { get; set; }

        public string error { get; set; }
    }
}