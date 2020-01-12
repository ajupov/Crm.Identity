using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Profiles.Models;

namespace Crm.Identity.UserInfo.Services
{
    public interface IUserInfoService
    {
        Task<Models.UserInfo> GetByScopesAsync(Profile profile, CancellationToken ct);
    }
}