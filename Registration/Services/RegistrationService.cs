//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UserPermission = Crm.Modules.Users.Models.UserPermission;
//
//namespace Identity.Registration.Services
//{
//    public class RegistrationService : IRegistrationService
//    {
//        private readonly IMailSender _mailService;
//        private readonly AccountsStorage _accountsStorage;
//        private readonly UsersStorage _usersStorage;
//        private readonly IdentitiesStorage _identitiesStorage;
//
//        public RegistrationService(
//            IMailSender mailService,
//            AccountsStorage accountsStorage,
//            UsersStorage usersStorage,
//            IdentitiesStorage identitiesStorage)
//        {
//            _mailService = mailService;
//            _accountsStorage = accountsStorage;
//            _usersStorage = usersStorage;
//            _identitiesStorage = identitiesStorage;
//        }
//
//        public async Task<Account> CreateAccountAsync()
//        {
//            var account = new Account
//            {
//                Id = default(int),
//                IsLocked = false,
//                IsDeleted = false,
//                CreateDate = DateTime.Now,
//                Settings = null
//            };
//
//            var entry = await _accountsStorage.AddAsync(account);
//            await _accountsStorage.SaveChangesAsync();
//
//            return entry.Entity;
//        }
//
//        public async Task<User> CreateUserAsync(Account account, string surname = null, string name = null, 
//            DateTime? birthDate = null, UserGender gender = UserGender.None)
//        {
//            var user = new User
//            {
//                Id = default(int),
//                AccountId = account.Id,
//                Surname = surname,
//                Name = name,
//                Patronymic = default(string),
//                BirthDate = birthDate,
//                Gender = gender,
//                AvatarUrl = null,
//                CreateDate = DateTime.Now,
//                IsLocked = false,
//                IsDeleted = false,
//                Permissions = new List<UserPermission>
//                {
//                    new UserPermission
//                    {
//                        Id = default(int),
//                        UserId = default(int),
//                        Permission = Modules.Users.Enums.UserPermission.AccountOwner
//                    }
//                }
//            };
//
//            var entry = await _usersStorage.AddAsync(user);
//            await _usersStorage.SaveChangesAsync();
//
//            return entry.Entity;
//        }
//
//        public async Task<Identity> CreateLoginIdentityAsync(User user, string login, string passwordHash)
//        {
//            var identity = new Identity
//            {
//                Id = default(int),
//                UserId = user.Id,
//                Type = IdentityType.LoginAndPassword,
//                Key = login,
//                PasswordHash = passwordHash,
//                IsPrimary = false,
//                IsVerified = true,
//                CreateDate = DateTime.Now
//            };
//
//            var entry = await _identitiesStorage.AddAsync(identity);
//            await _identitiesStorage.SaveChangesAsync();
//
//            return entry.Entity;
//        }
//
//        public async Task<Identity> CreateEmailIdentityAsync(User user, string email, string passwordHash, bool needVerify = true)
//        {
//            var now = DateTime.Now;
//            var code = Generator.GenerateAlphaNumbericString(256);
//
//            var identityByEmail = new Identity
//            {
//                Id = default(int),
//                UserId = user.Id,
//                Type = IdentityType.EmailAndPassword,
//                Key = email,
//                PasswordHash = passwordHash,
//                IsPrimary = true,
//                IsVerified = !needVerify,
//                CreateDate = now,
//                Tokens = new List<IdentityToken>
//                {
//                    new IdentityToken
//                    {
//                        Id = default(int),
//                        IdentityId = default(int),
//                        Value = code,
//                        CreateDate = now,
//                        ExpirationDate = now.AddDays(1),
//                        UseDate = null
//                    }
//                }
//            };
//
//            var entry = await _identitiesStorage.AddAsync(identityByEmail);
//            await _identitiesStorage.SaveChangesAsync();
//
//            return entry.Entity;
//        }
//
//        public async Task<Identity> CreateExternalIdentityAsync(User user, IdentityType identityType, string externalValue)
//        {
//            var identity = new Identity
//            {
//                Id = default(int),
//                UserId = user.Id,
//                Type = identityType,
//                Key = externalValue,
//                PasswordHash = null,
//                IsPrimary = false,
//                IsVerified = true,
//                CreateDate = DateTime.Now
//            };
//
//            var entry = await _identitiesStorage.AddAsync(identity);
//            await _identitiesStorage.SaveChangesAsync();
//
//            return entry.Entity;
//        }
//
//        public Task SendEmailConfirmationUrlAsync(string email, string verifyUrl)
//        {
//            const string subject = "Подтверждение почты";
//            var message = $"Пожалуйста, подтвердите свою почту, нажав на <a href='{verifyUrl}'>ссылку</a>.";
//
//            return _mailService.SendSystemAsync(email, subject, message);
//        }
//    }
//}