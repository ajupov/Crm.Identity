using System;
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

        public async Task RegisterAsync(
            string surname,
            string name,
            ProfileGender gender,
            DateTime birthDate,
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
                Name = name,
                BirthDate = birthDate,
                Gender = gender
            };

            profile.Id = await _profilesService.CreateAsync(profile, ct);

            var passwordHash = Password.ToPasswordHash(password);

            await CreateLoginIdentityAsync(profile.Id, login, passwordHash, ct);
            await CreateEmailIdentityAsync(profile.Id, email, passwordHash, ipAddress, userAgent, ct);
            await CreatePhoneIdentityAsync(profile.Id, phone, passwordHash, ipAddress, userAgent, ct);
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

            await _identityTokensService.CreateAsync(token, ct);
            await SendEmailConfirmationCodeAsync(email, code);
        }

        public async Task SendPhoneConfirmationSmsAsync(
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

            await _identityTokensService.CreateAsync(token, ct);
            await SendPhoneConfirmationCodeAsync(phone, code);
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

        private async Task CreatePhoneIdentityAsync(
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
            await SendPhoneConfirmationSmsAsync(phone, ipAddress, userAgent, ct);
        }

        private Task SendEmailConfirmationCodeAsync(string email, string code)
        {
            const string subject = "Email verification";
            var uri = string.Format(_settings.VerifyUriPattern, code);
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
    }
}