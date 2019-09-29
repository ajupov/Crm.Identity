using System.Linq;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.Profiles.Helpers
{
    public static class ProfilesSortingHelper
    {
        public static IOrderedQueryable<Profile> Sort(
            this IQueryable<Profile> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            return sortBy switch
            {
                nameof(Profile.Id) => (isDesc
                    ? queryable.OrderByDescending(x => x.Id)
                    : queryable.OrderBy(x => x.Id)),
                nameof(Profile.Surname) => (isDesc
                    ? queryable.OrderByDescending(x => x.Surname)
                    : queryable.OrderBy(x => x.Surname)),
                nameof(Profile.Name) => (isDesc
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name)),
                nameof(Profile.BirthDate) => (isDesc
                    ? queryable.OrderByDescending(x => x.BirthDate)
                    : queryable.OrderBy(x => x.BirthDate)),
                nameof(Profile.CreateDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.CreateDateTime)
                    : queryable.OrderBy(x => x.CreateDateTime)),
                nameof(Profile.ModifyDateTime) => (isDesc
                    ? queryable.OrderByDescending(x => x.ModifyDateTime)
                    : queryable.OrderBy(x => x.ModifyDateTime)),
                _ => queryable.OrderByDescending(x => x.CreateDateTime)
            };
        }
    }
}