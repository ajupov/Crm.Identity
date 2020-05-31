using System.Linq;

namespace Crm.Identity.Identities.Helpers
{
    public static class IdentitiesSortingHelper
    {
        public static IOrderedQueryable<Models.Identity> Sort(
            this IQueryable<Models.Identity> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            return sortBy switch
            {
                nameof(Models.Identity.Type) => isDesc
                    ? queryable.OrderByDescending(x => x.Type)
                    : queryable.OrderBy(x => x.Type),
                nameof(Models.Identity.CreateDateTime) => isDesc
                    ? queryable.OrderByDescending(x => x.CreateDateTime)
                    : queryable.OrderBy(x => x.CreateDateTime),
                nameof(Models.Identity.ModifyDateTime) => isDesc
                    ? queryable.OrderByDescending(x => x.ModifyDateTime)
                    : queryable.OrderBy(x => x.ModifyDateTime),
                _ => queryable.OrderByDescending(x => x.CreateDateTime)
            };
        }
    }
}
