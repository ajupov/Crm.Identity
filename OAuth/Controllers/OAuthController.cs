using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.String;
using Crm.Identity.Clients.Services;
using Crm.Identity.OAuth.Attributes.Security;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.Register;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Services;
using Crm.Identity.OAuth.ViewModels;
using Infrastructure.All.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Identity.OAuth.Controllers
{
    [SecurityHeaders]
    [Route("OAuth")]
    public class OAuthController : DefaultMvcController
    {
        private readonly IOAuthService _oauthService;
        private readonly IClientsService _clientsService;

        public OAuthController(IOAuthService oauthService, IClientsService clientsService)
        {
            _oauthService = oauthService;
            _clientsService = clientsService;
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
                request.redirect_uri, request.state, false);

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

            var model = new AuthorizeViewModel(request.client_id, request.response_type, request.scope,
                request.redirect_uri, request.state, false);

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

            var response = await _oauthService.AuthorizeAsync(request, UserAgent, IpAddress, ct);
            if (response.HasError)
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

            return RedirectToAction("Authorize");
        }

        // Return new tokens
        [HttpPost("Token")]
        public async Task<ActionResult<TokenResponse>> Token(TokenRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.ClientId, ct);
            if (client == null)
            {
                return BadRequest(request.ClientId);
            }

            if (client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return Forbid();
            }

            if (request.ClientSecret != client.ClientSecret)
            {
                return BadRequest(request.ClientSecret);
            }

            if (!Regex.IsMatch(request.RedirectUri, client.RedirectUriPattern))
            {
                return BadRequest(request.RedirectUri);
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.RedirectUri);
            }

            var response = await _oauthService.GetTokenAsync(request, HttpContext.User, UserAgent, IpAddress, ct);
            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return response;
        }
    }
}