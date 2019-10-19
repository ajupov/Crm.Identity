﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.Profiles.Services;
using Ajupov.Identity.Registration.Settings;
using Ajupov.Infrastructure.All.MailSending.MailSender;
using Ajupov.Infrastructure.All.SmsSending.SmsSender;
using Ajupov.Utils.All.Password;
using Infrastructure.All.Generator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Registration.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly VerifyEmailSettings _settings;
        private readonly ILogger<RegistrationService> _logger;
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly IMailSender _mailSender;
        private readonly ISmsSender _smsSender;

        public RegistrationService(
            IOptions<VerifyEmailSettings> settings,
            ILogger<RegistrationService> logger,
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService,
            IMailSender mailSender,
            ISmsSender smsSender)
        {
            _settings = settings.Value;
            _logger = logger;
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
            _mailSender = mailSender;
            _smsSender = smsSender;
        }

        public Task<bool> IsLoginExistsAsync(string login, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(login, IdentityType.LoginAndPassword, ct);
        }

        public Task<bool> IsEmailExistsAsync(string email, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(email, IdentityType.EmailAndPassword, ct);
        }

        public Task<bool> IsPhoneExistsAsync(string phone, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(phone, IdentityType.PhoneAndPassword, ct);
        }

        public async Task<Guid> RegisterAsync(
            string surname,
            string name,
            string login,
            string email,
            string phone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var profile = new Profile
            {
                Surname = surname,
                Name = name
            };

            profile.Id = await _profilesService.CreateAsync(profile, ct);

            var passwordHash = Password.ToPasswordHash(password);

            await CreateLoginIdentityAsync(profile.Id, login, passwordHash, ct);
            await CreateEmailIdentityAsync(profile.Id, email, passwordHash, ipAddress, userAgent, ct);
            return await CreatePhoneIdentityAsync(profile.Id, phone, passwordHash, ipAddress, userAgent, ct);
        }

        public async Task SendEmailConfirmationEmailAsync(
            string email,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = Generator.GenerateAlphaNumericString(256);
            var identityTypes = new[] {IdentityType.EmailAndPassword};
            var identity = await _identitiesService.GetByKeyAndTypesAsync(email, identityTypes, ct);

            var token = new IdentityToken
            {
                IdentityId = identity.Id,
                Type = IdentityTokenType.EmailValidation,
                Value = code,
                CreateDateTime = now,
                ExpirationDateTime = now.AddDays(1),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var id = await _identityTokensService.CreateAsync(token, ct);
            await SendEmailConfirmationCodeAsync(email, id, code);
        }

        public async Task<Guid> SendPhoneConfirmationSmsAsync(
            string phone,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = new Random().Next(0, 9999).ToString("0000");
            var identityTypes = new[] {IdentityType.PhoneAndPassword};
            var identity = await _identitiesService.GetByKeyAndTypesAsync(phone, identityTypes, ct);

            var token = new IdentityToken
            {
                IdentityId = identity.Id,
                Type = IdentityTokenType.PhoneValidation,
                Value = code,
                CreateDateTime = now,
                ExpirationDateTime = now.AddMinutes(10),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var id = await _identityTokensService.CreateAsync(token, ct);
            await SendPhoneConfirmationCodeAsync(phone, code);

            return id;
        }

        public async Task<bool> VerifyEmailAsync(Guid tokenId, string code, CancellationToken ct)
        {
            return await VerifyIdentityAsync(tokenId, code, IdentityTokenType.EmailValidation, ct);
        }

        public async Task<bool> VerifyPhoneAsync(Guid tokenId, string code, CancellationToken ct)
        {
            return await VerifyIdentityAsync(tokenId, code, IdentityTokenType.PhoneValidation, ct);
        }

        private async Task CreateLoginIdentityAsync(
            Guid profileId,
            string login,
            string passwordHash,
            CancellationToken ct)
        {
            var loginIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.LoginAndPassword,
                Key = login,
                PasswordHash = passwordHash,
                IsVerified = true
            };

            await _identitiesService.CreateAsync(loginIdentity, ct);
        }

        private async Task CreateEmailIdentityAsync(
            Guid profileId,
            string email,
            string passwordHash,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var emailIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.EmailAndPassword,
                Key = email,
                PasswordHash = passwordHash,
                IsVerified = false
            };

            await _identitiesService.CreateAsync(emailIdentity, ct);
            await SendEmailConfirmationEmailAsync(email, ipAddress, userAgent, ct);
        }

        private async Task<Guid> CreatePhoneIdentityAsync(
            Guid profileId,
            string phone,
            string passwordHash,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var phoneIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.PhoneAndPassword,
                Key = phone,
                PasswordHash = passwordHash,
                IsVerified = false
            };

            await _identitiesService.CreateAsync(phoneIdentity, ct);
            var id = await SendPhoneConfirmationSmsAsync(phone, ipAddress, userAgent, ct);

            return id;
        }

        private Task SendEmailConfirmationCodeAsync(string email, Guid tokenId, string code)
        {
            const string subject = "Email verification";
            var uri = string.Format(_settings.VerifyUriPattern, tokenId, code);
            var message = $"Click <a href='{uri}'>here</a> for verify email.";

            try
            {
                return _mailSender.SendAsync(_settings.FromName, _settings.FromAddress, subject, new[] {email}, true,
                    message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.CompletedTask;
            }
        }

        private Task SendPhoneConfirmationCodeAsync(string phone, string code)
        {
            var message = $"Confirmation code - {code}.";

            try
            {
                return _smsSender.SendAsync(phone, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.CompletedTask;
            }
        }

        private async Task<bool> VerifyIdentityAsync(
            Guid tokenId,
            string code,
            IdentityTokenType tokenType,
            CancellationToken ct)
        {
            var token = await _identityTokensService.GetAsync(tokenId, ct);
            if (token == null ||
                token.UseDateTime.HasValue ||
                token.ExpirationDateTime < DateTime.UtcNow ||
                token.Type != tokenType ||
                token.Value != code)
            {
                return false;
            }

            await _identityTokensService.SetIsUsedAsync(tokenId, ct);
            await _identitiesService.VerifyAsync(new[] {token.IdentityId}, ct);

            return true;
        }
    }
}