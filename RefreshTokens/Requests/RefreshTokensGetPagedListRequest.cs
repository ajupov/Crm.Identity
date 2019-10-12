using System;

namespace Ajupov.Identity.RefreshTokens.Requests
{
    public class RefreshTokensGetPagedListRequest
    {
        public Guid ProfileId { get; set; }

        public DateTime? MinCreateDateTime { get; set; }

        public DateTime? MaxCreateDateTime { get; set; }

        public DateTime? MinExpirationDateTime { get; set; }

        public DateTime? MaxExpirationDateTime { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}