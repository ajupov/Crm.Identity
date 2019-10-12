using System.Linq;
using Ajupov.Identity.OAuthClients.Models;

namespace Ajupov.Identity.OAuthClients.Helpers
{
    public static class OAuthClientsSortingHelper
    {
        public static IOrderedQueryable<OAuthClient> Sort(
            this IQueryable<OAuthClient> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            return sortBy switch
            {
                nameof(OAuthClient.CreateDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.CreateDateTime)
                    : queryable.OrderBy(x => x.CreateDateTime)),
                nameof(OAuthClient.ModifyDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.ModifyDateTime)
                    : queryable.OrderBy(x => x.ModifyDateTime)),
                _ => queryable.OrderByDescending(x => x.CreateDateTime)
            };
        }
    }
}