using System.Linq;
using Ajupov.Identity.Clients.Models;

namespace Ajupov.Identity.Clients.Helpers
{
    public static class ClientsSortingHelper
    {
        public static IOrderedQueryable<Client> Sort(this IQueryable<Client> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            return sortBy switch
            {
                nameof(Client.Id) => (isDesc
                    ? queryable.OrderByDescending(x => x.Id)
                    : queryable.OrderBy(x => x.Id)),
                nameof(Client.ClientId) => (isDesc
                    ? queryable.OrderByDescending(x => x.ClientId)
                    : queryable.OrderBy(x => x.ClientId)),
                nameof(Client.CreateDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.CreateDateTime)
                    : queryable.OrderBy(x => x.CreateDateTime)),
                nameof(Client.ModifyDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.ModifyDateTime)
                    : queryable.OrderBy(x => x.ModifyDateTime)),
                _ => queryable.OrderByDescending(x => x.CreateDateTime)
            };
        }
    }
}