using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Clients.Services;
using Ajupov.Identity.OAuth.Attributes.Security;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Register;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Services;
using Ajupov.Identity.OAuth.ViewModels;
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
        private readonly IClientsService _clientsService;
        private readonly IRegistrationService _registrationService;

        public OAuthController(
            IOAuthService oauthService,
            IClientsService clientsService,
            IRegistrationService registrationService)
        {
            _oauthService = oauthService;
            _clientsService = clientsService;
            _registrationService = registrationService;
        }

        // Show authorize form
        [HttpGet("Authorize")]
        public async Task<ActionResult> Authorize(GetAuthorizeRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.client_id, ct);
            if (client == null || client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return BadRequest("Client not found");
            }

            if (!Regex.IsMatch(request.redirect_uri, client.RedirectUriPattern))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.redirect_uri);
            }

            var model = new AuthorizeViewModel(request.client_id, request.response_type, request.scope,
                request.redirect_uri, request.state);

            return View("~/OAuth/Views/Authorize.cshtml", model);
        }

        // Show registration form
        [HttpGet("Register")]
        public async Task<ActionResult> Register(GetRegisterRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.client_id, ct);
            if (client == null || client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return BadRequest("Client not found");
            }

            if (!Regex.IsMatch(request.redirect_uri, client.RedirectUriPattern))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.redirect_uri);
            }

            var model = new RegisterViewModel(request.client_id, request.response_type, request.scope,
                request.redirect_uri, request.state);

            return View("~/OAuth/Views/Register.cshtml", model);
        }

        // Redirect with code or tokens
        [HttpPost("Authorize")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Authorize([FromForm] PostAuthorizeRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.client_id, ct);
            if (client == null || client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return BadRequest("Client not found");
            }

            if (!Regex.IsMatch(request.redirect_uri, client.RedirectUriPattern))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.redirect_uri);
            }

            var response = await _oauthService.AuthorizeAsync(request.Login, request.Password, request.IsRemember,
                request.response_type, request.redirect_uri, UserAgent, IpAddress, ct);
            if (response.IsInvalidCredentials)
            {
                var model = new AuthorizeViewModel(request.client_id, request.response_type, request.scope,
                    request.redirect_uri, request.state, true);

                return View("~/OAuth/Views/Authorize.cshtml", model);
            }

            return Redirect(response.RedirectUri);
        }

        // Redirect with code or tokens
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([FromForm] PostRegisterRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.client_id, ct);
            if (client == null || client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return BadRequest("Client not found");
            }

            if (!Regex.IsMatch(request.redirect_uri, client.RedirectUriPattern))
            {
                return BadRequest("Invalid redirect uri");
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.redirect_uri);
            }

            var isLoginExists = await _registrationService.IsLoginExistsAsync(request.Login, ct);
            var isEmailExists = await _registrationService.IsEmailExistsAsync(request.Email, ct);
            var isPhoneExists = await _registrationService.IsPhoneExistsAsync(request.Phone, ct);

            if (isLoginExists || isEmailExists || isPhoneExists)
            {
                var model = new RegisterViewModel(request.client_id, request.response_type, request.scope,
                    request.redirect_uri, request.state, isLoginExists, isEmailExists, isPhoneExists);

                return View("~/OAuth/Views/Register.cshtml", model);
            }

            await _registrationService.RegisterAsync(request.Surname, request.Name, request.Gender, request.BirthDate,
                request.Login, request.Email, request.Phone, request.Password, IpAddress, UserAgent, ct);

            var response = await _oauthService.AuthorizeAsync(request.Login, request.Password, false,
                request.response_type, request.redirect_uri, UserAgent, IpAddress, ct);
            if (response.IsInvalidCredentials)
            {
                var model = new AuthorizeViewModel(request.client_id, request.response_type, request.scope,
                    request.redirect_uri, request.state, true);

                return View("~/OAuth/Views/Authorize.cshtml", model);
            }

            return Redirect(response.RedirectUri);
        }

        // Return new tokens
        [HttpPost("Token")]
        public async Task<ActionResult<TokenResponse>> Token(TokenRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.client_id, ct);
            if (client == null)
            {
                return BadRequest(request.client_id);
            }

            if (client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return Forbid();
            }

            if (request.client_secret != client.ClientSecret)
            {
                return BadRequest(request.client_secret);
            }

            if (!Regex.IsMatch(request.redirect_uri, client.RedirectUriPattern))
            {
                return BadRequest(request.redirect_uri);
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.redirect_uri);
            }

            var response = await _oauthService.GetTokenAsync(request.grant_type, request.code, request.redirect_uri,
                request.username, request.password, request.refresh_token, UserAgent, IpAddress, ct);
            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return response;
        }
    }
}