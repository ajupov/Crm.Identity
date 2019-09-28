//using System;
//using System.Threading.Tasks;
//using Identity.Identities.Models;
//using Identity.Users.Models;
//
//namespace Identity.Registration.Services
//{
//    public interface IRegistrationService
//    {
//        Task<User> CreateUserAsync(
//            string surname = null,
//            string name = null,
//            DateTime? birthDate = null,
//            UserGender gender);
//
//        Task<Identities.Models.Identity> CreateLoginIdentityAsync(User user, string login, string passwordHash);
//
//        Task<Identities.Models.Identity> CreateEmailIdentityAsync(User user, string email, string passwordHash, bool needVerify = true);
//
//        Task<Identities.Models.Identity> CreateExternalIdentityAsync(User user, IdentityType identityType, string externalValue);
//
//        Task SendEmailConfirmationUrlAsync(string email, string verifyUrl);
//    }
//}