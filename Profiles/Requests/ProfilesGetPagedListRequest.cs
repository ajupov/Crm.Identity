using System;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.Profiles.Requests
{
    public class ProfilesGetPagedListRequest
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public DateTime? MinBirthDate { get; set; }

        public DateTime? MaxBirthDate { get; set; }

        public ProfileGender? Gender { get; set; }

        public bool? IsLocked { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public DateTime? MinModifyDate { get; set; }

        public DateTime? MaxModifyDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}