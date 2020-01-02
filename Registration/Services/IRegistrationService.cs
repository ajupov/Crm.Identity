﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Registration.Services
{
    public interface IRegistrationService
    {
        Task<Guid> RegisterAsync(string country, string surname,
            string name,
            string login,
            string email,
            string phone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct);
    }
}