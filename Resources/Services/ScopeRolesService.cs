using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Resources.Services
{
    public class ScopeRolesService : IScopeRolesService
    {
        private bool _isLoaded;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private Dictionary<string, string[]> _data = new Dictionary<string, string[]>();

        public async Task<Dictionary<string, string[]>> GetAsync()
        {
            if (_isLoaded)
            {
                return _data;
            }

            await _semaphore.WaitAsync();

            try
            {
                if (_isLoaded)
                {
                    return _data;
                }

                _data = await LoadAsync();
                _isLoaded = true;
            }
            finally
            {
                _semaphore.Release();
            }

            return _data;
        }

        private static Task<Dictionary<string, string[]>> LoadAsync()
        {
            return Task.FromResult(new Dictionary<string, string[]>
            {
                {"all", new[] {"AccountOwning", "ProductsManagement", "LeadsManagement", "SalesManagement"}}
            });
        }
    }
}