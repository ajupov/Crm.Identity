using System;
using System.Collections.Generic;
using Crm.Identity.Identities.Models;

namespace Crm.Identity.Identities.Requests
{
    public class IdentitiesGetPagedListRequest
    {
        public Guid ProfileId { get; set; }

        public List<IdentityType> Types { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? MinCreateDateTime { get; set; }

        public DateTime? MaxCreateDateTime { get; set; }

        public DateTime? MinModifyDateTime { get; set; }

        public DateTime? MaxModifyDateTime { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}