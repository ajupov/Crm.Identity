using System.Linq;

namespace Ajupov.Identity.Identities.Helpers
{
    public static class IdentitiesSortingHelper
    {
        public static IOrderedQueryable<Models.Identity> Sort(
            this IQueryable<Models.Identity> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Models.Identity.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Models.Identity.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(Models.Identity.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                case nameof(Models.Identity.ModifyDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.ModifyDateTime)
                        : queryable.OrderBy(x => x.ModifyDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}