using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Attributes.Security;
using Ajupov.Identity.OAuth.Extensions;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Register;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.VerifyEmail;
using Ajupov.Identity.OAuth.Models.VerifyPhone;
using Ajupov.Identity.OAuth.Services;
using Ajupov.Identity.OAuth.Validators;
using Ajupov.Identity.OAuth.ViewModels;
using Ajupov.Identity.OAuthClients.Services;
using Ajupov.Identity.Registration.Services;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Utils.All.String;
using Microsoft.AspNetCore.Mvc;

namespace Ajupov.Identity.OAuth.Controllers
{
    [SecurityHeaders]
    [Route("OAuth")]
    public class OAuthController : DefaultMvcController
    {
        private readonly IOAuthService _oauthService;
        private readonly IOAuthClientsService _ioAuthClientsService;
        private readonly IRegistrationService _registrationService;

        public OAuthController(
            IOAuthService oauthService,
            IOAuthClientsService ioAuthClientsService,
            IRegistrationService registrationService)
        {
            _oauthService = oauthService;
            _ioAuthClientsService = ioAuthClientsService;
            _registrationService = registrationService;
        }

        [HttpGet("Authorize")]
        public async Task<ActionResult> Authorize(GetAuthorizeRequest request, CancellationToken ct)
        {
            var client = await _ioAuthClientsService.GetByClientIdAsync(request.client_id, ct);
            if (!client.IsValid())
            {
                return BadRequest("Client not found");
            }

            if (!client.IsMatchRedirectUri(request))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (!client.IsScopesInclude(request.scope))
            {
                return BadRequest("Invalid scopes");
            }

            var model = new AuthorizeViewModel(
                request.client_id,
                request.response_type,
                request.scope,
                request.redirect_uri,
                request.state,
                request.IsInvalidCredentials);

            return View("~/OAuth/Views/Authorize.cshtml", model);
        }

        [HttpGet("Register")]
        public async Task<ActionResult> Register(GetRegisterRequest request, CancellationToken ct)
        {
            var client = await _ioAuthClientsService.GetByClientIdAsync(request.client_id, ct);
            if (!client.IsValid())
            {
                return BadRequest("Client not found");
            }

            if (!client.IsMatchRedirectUri(request))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (!client.IsScopesInclude(request.scope))
            {
                return BadRequest("Invalid scopes");
            }

            var model = new RegisterViewModel(
                request.client_id,
                request.response_type,
                request.scope,
                request.redirect_uri,
                request.state,
                request.Surname,
                request.Name,
                request.Login,
                request.Email,
                request.Phone,
                request.IsLoginExists,
                request.IsEmailExists,
                request.IsPhoneExists);

            return View("~/OAuth/Views/Register.cshtml", model);
        }

        [HttpPost("Authorize")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Authorize([FromForm] PostAuthorizeRequest request, CancellationToken ct)
        {
            var client = await _ioAuthClientsService.GetByClientIdAsync(request.client_id, ct);
            if (!client.IsValid())
            {
                return BadRequest("Client not found");
            }

            if (!client.IsMatchRedirectUri(request))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (!client.IsScopesInclude(request.scope))
            {
                return BadRequest("Invalid scopes");
            }

            var response = await _oauthService.AuthorizeAsync(
                request.Login,
                request.Password,
                request.response_type,
                request.redirect_uri,
                request.state,
                UserAgent,
                IpAddress,
                request.scope.ToList(),
                ct);

            if (!response.IsInvalidCredentials)
            {
                return Redirect(response.CallbackUri);
            }

            var newRequest = new GetAuthorizeRequest
            {
                client_id = request.client_id,
                response_type = request.response_type,
                scope = request.scope,
                state = request.state,
                redirect_uri = request.redirect_uri,
                IsInvalidCredentials = true
            };

            return RedirectToAction("Authorize", newRequest);
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([FromForm] PostRegisterRequest request, CancellationToken ct)
        {
            var client = await _ioAuthClientsService.GetByClientIdAsync(request.client_id, ct);
            if (!client.IsValid())
            {
                return BadRequest("Client not found");
            }

            if (!client.IsMatchRedirectUri(request))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (!client.IsScopesInclude(request.scope))
            {
                return BadRequest("Invalid scopes");
            }

            var isLoginExists = await _registrationService.IsLoginExistsAsync(request.Login, ct);
            var isEmailExists = await _registrationService.IsEmailExistsAsync(request.Email, ct);
            var isPhoneExists = await _registrationService.IsPhoneExistsAsync(request.Phone, ct);

            if (isLoginExists || isEmailExists || isPhoneExists)
            {
                var newRegisterRequest = new GetRegisterRequest
                {
                    client_id = request.client_id,
                    response_type = request.response_type,
                    scope = request.scope,
                    state = request.state,
                    redirect_uri = request.redirect_uri,
                    Surname = request.Surname,
                    Name = request.Name,
                    Login = request.Login,
                    Email = request.Email,
                    Phone = request.Phone,
                    IsLoginExists = isLoginExists,
                    IsEmailExists = isEmailExists,
                    IsPhoneExists = isPhoneExists
                };

                return RedirectToAction("Register", newRegisterRequest);
            }

            var phoneIdentityTokenId = await _registrationService.RegisterAsync(
                request.Surname,
                request.Name,
                request.Login,
                request.Email,
                request.Phone,
                request.Password,
                IpAddress,
                UserAgent,
                ct);

            var authorizeResponse = await _oauthService.AuthorizeAsync(
                request.Login,
                request.Password,
                request.response_type,
                request.redirect_uri,
                request.state,
                UserAgent,
                IpAddress,
                request.scope.ToList(),
                ct);

            if (authorizeResponse.IsInvalidCredentials)
            {
                var newAuthorizeRequest = new GetAuthorizeRequest
                {
                    client_id = request.client_id,
                    response_type = request.response_type,
                    scope = request.scope,
                    state = request.state,
                    redirect_uri = request.redirect_uri,
                    IsInvalidCredentials = true
                };

                return RedirectToAction("Authorize", newAuthorizeRequest);
            }

            var getVerifyPhoneRequest = new GetVerifyPhoneRequest
            {
                TokenId = phoneIdentityTokenId,
                CallbackUri = authorizeResponse.CallbackUri,
                IsInvalidCode = false
            };

            return RedirectToAction("VerifyPhone", getVerifyPhoneRequest);
        }

        [HttpPost("Token")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<TokenResponse>> Token([FromForm] TokenRequest request, CancellationToken ct)
        {
            var client = await _ioAuthClientsService.GetByClientIdAsync(request.client_id, ct);
            if (!client.IsValid())
            {
                return BadRequest("Client not found");
            }

            if (!client.IsMatchRedirectUri(request))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (!client.IsCorrectSecret(request))
            {
                return BadRequest("Invalid client secret");
            }

            var response = await _oauthService.GetTokenAsync(
                request.grant_type,
                request.code,
                request.redirect_uri,
                request.username,
                request.password,
                request.refresh_token,
                UserAgent,
                IpAddress,
                client.Scopes.Select(x => x.Value).ToList(),
                ct);

            if (!response.error.IsEmpty())
            {
                return BadRequest(response.error);
            }

            return response;
        }

        [HttpGet("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(VerifyEmailRequest request, CancellationToken ct)
        {
            var isVerified = await _registrationService.VerifyEmailAsync(request.TokenId, request.Code, ct);
            if (!isVerified)
            {
                return BadRequest("Invalid code");
            }

            return View("~/OAuth/Views/EmailVerified.cshtml");
        }

        [HttpGet("VerifyPhone")]
        public ActionResult VerifyPhone(GetVerifyPhoneRequest request, CancellationToken ct)
        {
            var model = new VerifyPhoneViewModel(
                request.TokenId,
                request.CallbackUri,
                request.IsInvalidCode);

            return View("~/OAuth/Views/VerifyPhone.cshtml", model);
        }

        [HttpPost("VerifyPhone")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhone([FromForm] PostVerifyPhoneRequest request, CancellationToken ct)
        {
            var isVerified = await _registrationService.VerifyPhoneAsync(request.TokenId, request.Code, ct);
            if (!isVerified)
            {
                var getVerifyPhoneRequest = new GetVerifyPhoneRequest
                {
                    TokenId = request.TokenId,
                    CallbackUri = request.CallbackUri,
                    IsInvalidCode = true
                };

                return RedirectToAction("VerifyPhone", getVerifyPhoneRequest);
            }

            if (request.CallbackUri.IsEmpty())
            {
                return View("~/OAuth/Views/PhoneVerified.cshtml");
            }

            return Redirect(request.CallbackUri);
        }
    }
}