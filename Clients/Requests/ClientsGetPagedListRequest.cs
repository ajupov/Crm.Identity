using System;

namespace Crm.Identity.Clients.Requests
{
    public class ClientsGetPagedListRequest
    {
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