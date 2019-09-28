//using System;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using Crm.Infrastructure.Mvc;
//using Identity.Clients.Services;
//using Identity.Identities.Models;
//using Identity.Identities.Storages;
//using Identity.OAuth.Services;
//using Identity.OAuth.ViewModels;
//using Identity.Registration.Models;
//using Identity.Registration.Services;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.EntityFrameworkCore;
//
//namespace Identity.Registration.Controllers
//{
//    [Route("Registration")]
//    public class RegistrationController : DefaultApiController
//    {
//        private readonly IRegistrationService _registrationService;
//        private readonly IClientsService _clientsService;
//
//        private readonly IdentitiesStorage _identitiesStorage;
//        private readonly IAuthenticationService _authenticationService;
//
//        public RegistrationController(
//            IRegistrationService registrationService,
//            IdentitiesStorage identitiesStorage,
//            IAuthenticationService authenticationService,
//            IClientsService clientsService)
//        {
//            _registrationService = registrationService;
//            _identitiesStorage = identitiesStorage;
//            _authenticationService = authenticationService;
//            _clientsService = clientsService;
//        }
//
//        // Show registration form
//        [HttpGet("Register")]
//        public async Task<ActionResult> Register(GetRegisterRequest request, CancellationToken ct)
//        {
//            var client = await _clientsService.GetByClientIdAsync(request.ClientId, ct);
//            if (client == null)
//            {
//                return BadRequest(request.ClientId);
//            }
//
//            if (client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
//                client.RedirectUriPattern.IsEmpty())
//            {
//                return Forbid();
//            }
//
//            if (!Regex.IsMatch(request.RedirectUri, client.RedirectUriPattern))
//            {
//                return BadRequest(request.RedirectUri);
//            }
//
//            if (_oauthService.IsAuthorized(HttpContext.User))
//            {
//                return Redirect(request.RedirectUri);
//            }
//
//            var model = new AuthorizeViewModel(request.ResponseType, request.RedirectUri);
//
//            return View("~/OAuth/Views/Authorize.cshtml", model);
//        }
//
//        
//        [HttpPost("Register")]
//        public async Task<RegisterResponseModel> Register(RegisterRequestModel model)
//        {
//            var email = model.Email.TrimToLower();
//
//            var isExistByEmail = await IsExistByEmailAsync(email).ConfigureAwait(false);
//            if (isExistByEmail)
//            {
//                return new RegisterResponseModel
//                {
//                    Errors = new List<string> {"Email уже занят"}
//                };
//            }
//
//            var account = await _registrationService.CreateAccountAsync().ConfigureAwait(false);
//            var user = await _registrationService.CreateUserAsync(account, null, model.Name.Trim())
//                .ConfigureAwait(false);
//
//            var passwordHash = model.Password.ToPasswordHash();
//            var emailIdentity = await _registrationService.CreateEmailIdentityAsync(user, email, passwordHash)
//                .ConfigureAwait(false);
//
//            var confirmEmailRequestModel = new ConfirmEmailRequestModel
//            {
//                IdentityId = emailIdentity.Id,
//                Code = emailIdentity.Tokens.FirstOrDefault()?.Value
//            };
//
//            var verifyUrl = GenerateUrl("Registration", nameof(VerifyEmail), confirmEmailRequestModel);
//            await _registrationService.SendEmailConfirmationUrlAsync(email, verifyUrl).ConfigureAwait(false);
//
//            await _authenticationService.SignInAsync(HttpContext, user, false).ConfigureAwait(false);
//
//            return new RegisterResponseModel();
//        }
//
//        [HttpGet(Route.DefaultAction)]
//        [ValidateModel]
//        public async Task<IActionResult> VerifyEmail([FromQuery] ConfirmEmailRequestModel model)
//        {
//            if (model.IdentityId == 0 || string.IsNullOrEmpty(model.Code))
//            {
//                return BadRequestWithError("Произошла ошибка");
//            }
//
//            var identity = await GetIdentityAsync(model.IdentityId).ConfigureAwait(false);
//            if (identity == null)
//            {
//                return NotFound();
//            }
//
//            var token = await GetTokenAsync(model.IdentityId, model.Code).ConfigureAwait(false);
//            if (token == null)
//            {
//                return NotFound();
//            }
//
//            if (token.UseDate.HasValue)
//            {
//                return BadRequestWithError("Невозможно подтвердить Email");
//            }
//
//            if (token.ExpirationDate < DateTime.Now)
//            {
//                return BadRequestWithError("Срок для подтвержения Email истек");
//            }
//
//            token.UseDate = DateTime.Now;
//            identity.IsVerified = true;
//
//            _identitiesStorage.Update(token);
//            _identitiesStorage.Update(identity);
//
//            await _identitiesStorage.SaveChangesAsync().ConfigureAwait(false);
//
//            return RedirectToHome();
//        }
//
//        #region NonActions
//
//        [NonAction]
//        private Task<bool> IsExistByLoginAsync(string value)
//        {
//            return _identitiesStorage.Identities.AnyAsync(
//                x => x.Type == IdentityType.LoginAndPassword && x.Key == value);
//        }
//
//        [NonAction]
//        private Task<bool> IsExistByEmailAsync(string value)
//        {
//            return _identitiesStorage.Identities.AnyAsync(
//                x => x.Type == IdentityType.EmailAndPassword && x.Key == value);
//        }
//
//        [NonAction]
//        private Task<Identity> GetIdentityAsync(int identityId)
//        {
//            return _identitiesStorage.Identities.FindAsync(identityId);
//        }
//
//        [NonAction]
//        private Task<IdentityToken> GetTokenAsync(int identityId, string code)
//        {
//            return _identitiesStorage.IdentityTokens.FirstOrDefaultAsync(x =>
//                x.IdentityId == identityId && x.Value == code);
//        }
//
//        #endregion
//    }
//
//    public class GetRegisterRequest
//    {
//        public string ClientId { get; set; }
//        public string RedirectUri { get; set; }
//    }
//}