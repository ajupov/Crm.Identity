﻿using System.Linq;
using Crm.Identity.Areas.RefreshTokens.Models;

namespace Crm.Identity.Areas.RefreshTokens.Helpers
{
    public static class RefreshTokensSortingHelper
    {
        public static IOrderedQueryable<RefreshToken> Sort(
            this IQueryable<RefreshToken> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            return sortBy switch
            {
                nameof(RefreshToken.CreateDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.CreateDateTime)
                    : queryable.OrderBy(x => x.CreateDateTime)),
                nameof(RefreshToken.ExpirationDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.ExpirationDateTime)
                    : queryable.OrderBy(x => x.ExpirationDateTime)),
                _ => queryable.OrderByDescending(x => x.CreateDateTime)
            };
        }
    }
}