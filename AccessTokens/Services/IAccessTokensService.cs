using System.Collections.Generic;
using Crm.Identity.Claims.Models;

namespace Crm.Identity.AccessTokens.Services
{
    public interface IAccessTokensService
    {
        string Create(string audience, IEnumerable<Claim> claimModels);

        List<Claim> Read(string tokenString);
    }
}