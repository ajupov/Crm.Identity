using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Utils.All.String;
using Crm.Identity.Email.Services;
using Crm.Identity.Identities.Services;
using Crm.Identity.OAuth.Attributes.Security;
using Crm.Identity.OAuth.Extensions;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.ChangeEmail;
using Crm.Identity.OAuth.Models.ChangePassword;
using Crm.Identity.OAuth.Models.ChangePhone;
using Crm.Identity.OAuth.Models.Register;
using Crm.Identity.OAuth.Models.ResetPassword;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Models.VerifyEmail;
using Crm.Identity.OAuth.Models.VerifyPhone;
using Crm.Identity.OAuth.Services;
using Crm.Identity.OAuth.Validators;
using Crm.Identity.OAuth.ViewModels.Authorize;
using Crm.Identity.OAuth.ViewModels.ChangeEmail;
using Crm.Identity.OAuth.ViewModels.ChangePassword;
using Crm.Identity.OAuth.ViewModels.ChangePhone;
using Crm.Identity.OAuth.ViewModels.Register;
using Crm.Identity.OAuth.ViewModels.ResetPassword;
using Crm.Identity.OAuth.ViewModels.VerifyPhone;
using Crm.Identity.OAuthClients.Services;
using Crm.Identity.Password.Services;
using Crm.Identity.Phone.Services;
using Crm.Identity.Registration.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Identity.OAuth.Controllers
{
    [SecurityHeaders]
    [Area("OAuth")]
    [Route("OAuth")]
    public class OAuthController : DefaultMvcController
    {
        private readonly IOAuthService _oauthService;
        private readonly IOAuthClientsService _ioAuthClientsService;
        private readonly IRegistrationService _registrationService;
        private readonly IIdentityStatusService _identityStatusService;
        private readonly IEmailChangeService _emailChangeService;
        private readonly IPhoneChangeService _phoneChangeService;
        private readonly IPasswordChangeService _passwordChangeService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPhoneVerificationService _phoneVerificationService;

        public OAuthController(
            IOAuthService oauthService,
            IOAuthClientsService ioAuthClientsService,
            IRegistrationService registrationService,
            IIdentityStatusService identityStatusService,
            IEmailChangeService emailChangeService,
            IPhoneChangeService phoneChangeService,
            IPasswordChangeService passwordChangeService,
            IPasswordResetService passwordResetService,
            IEmailVerificationService emailVerificationService,
            IPhoneVerificationService phoneVerificationService)
        {
            _oauthService = oauthService;
            _ioAuthClientsService = ioAuthClientsService;
            _registrationService = registrationService;
            _identityStatusService = identityStatusService;
            _emailChangeService = emailChangeService;
            _phoneChangeService = phoneChangeService;
            _passwordChangeService = passwordChangeService;
            _passwordResetService = passwordResetService;
            _emailVerificationService = emailVerificationService;
            _phoneVerificationService = phoneVerificationService;
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
                request.IsPasswordsNotEqual,
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
                request.Country,
                request.Login,
                request.Password,
                request.response_type,
                request.redirect_uri,
                request.state,
                IpAddress,
                UserAgent,
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

            if (request.Password != request.PasswordConfirmation)
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
                    IsPasswordsNotEqual = true
                };

                return RedirectToAction("Register", newRegisterRequest);
            }

            var isLoginExists = await _identityStatusService.IsLoginExistsAsync(request.Login, ct);
            var isEmailExists = await _identityStatusService.IsEmailExistsAsync(request.Email, ct);
            var isPhoneExists = await _identityStatusService.IsPhoneExistsAsync(request.Phone, ct);

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
                request.Country,
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
                request.Country,
                request.Login,
                request.Password,
                request.response_type,
                request.redirect_uri,
                request.state,
                IpAddress,
                UserAgent,
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
                request.username,
                request.password,
                request.refresh_token,
                IpAddress,
                UserAgent,
                client.Scopes.Select(x => x.Value).ToList(),
                ct);

            if (!response.error.IsEmpty())
            {
                return BadRequest(response.error);
            }

            return response;
        }

        [HttpGet("ChangeEmail")]
        public ActionResult ChangeEmail(GetChangeEmailRequest request)
        {
            var model = new ChangeEmailViewModel(
                request.OldEmail,
                request.NewEmail,
                request.IsEmailNotChanged,
                request.IsEmailExists,
                request.IsInvalidCredentials);

            return View("~/OAuth/Views/ChangeEmail.cshtml", model);
        }

        [HttpPost("ChangeEmail")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail([FromForm] PostChangeEmailRequest request, CancellationToken ct)
        {
            if (request.OldEmail.Trim().ToLower() == request.NewEmail.Trim().ToLower())
            {
                var getChangeEmailRequest = new GetChangeEmailRequest
                {
                    OldEmail = request.OldEmail,
                    NewEmail = request.NewEmail,
                    IsEmailNotChanged = true
                };

                return RedirectToAction("ChangeEmail", getChangeEmailRequest);
            }

            var isEmailExists = await _identityStatusService.IsEmailExistsAsync(request.NewEmail, ct);
            if (isEmailExists)
            {
                var getChangeEmailRequest = new GetChangeEmailRequest
                {
                    OldEmail = request.OldEmail,
                    NewEmail = request.NewEmail,
                    IsEmailExists = true
                };

                return RedirectToAction("ChangeEmail", getChangeEmailRequest);
            }

            var response = await _emailChangeService.ChangeAsync(
                request.OldEmail,
                request.NewEmail,
                request.Password,
                IpAddress,
                UserAgent,
                ct);

            if (response.IsInvalidCredentials)
            {
                var getChangeEmailRequest = new GetChangeEmailRequest
                {
                    OldEmail = request.OldEmail,
                    NewEmail = request.NewEmail,
                    IsInvalidCredentials = true
                };

                return RedirectToAction("ChangeEmail", getChangeEmailRequest);
            }

            return View("~/OAuth/Views/ChangeEmailConfirmation.cshtml");
        }

        [HttpGet("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(VerifyEmailRequest request, CancellationToken ct)
        {
            var isVerified = await _emailVerificationService.VerifyAsync(request.TokenId, request.Code, ct);
            if (!isVerified)
            {
                return BadRequest("Invalid code");
            }

            return View("~/OAuth/Views/VerifyEmail.cshtml");
        }

        [HttpGet("ChangePhone")]
        public ActionResult ChangePhone(GetChangePhoneRequest request)
        {
            var model = new ChangePhoneViewModel(
                request.OldPhone,
                request.NewPhone,
                request.IsPhoneNotChanged,
                request.IsPhoneExists,
                request.IsInvalidCredentials);

            return View("~/OAuth/Views/ChangePhone.cshtml", model);
        }

        [HttpPost("ChangePhone")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePhone([FromForm] PostChangePhoneRequest request, CancellationToken ct)
        {
            if (request.OldPhone.Trim().ToLower() == request.NewPhone.Trim().ToLower())
            {
                var getChangePhoneRequest = new GetChangePhoneRequest
                {
                    OldPhone = request.OldPhone,
                    NewPhone = request.NewPhone,
                    IsPhoneNotChanged = true
                };

                return RedirectToAction("ChangePhone", getChangePhoneRequest);
            }

            var isPhoneExists = await _identityStatusService.IsPhoneExistsAsync(request.NewPhone, ct);
            if (isPhoneExists)
            {
                var getChangePhoneRequest = new GetChangePhoneRequest
                {
                    OldPhone = request.OldPhone,
                    NewPhone = request.NewPhone,
                    IsPhoneExists = true
                };

                return RedirectToAction("ChangePhone", getChangePhoneRequest);
            }

            var response = await _phoneChangeService.ChangeAsync(
                request.Country,
                request.OldPhone,
                request.NewPhone,
                request.Password,
                IpAddress,
                UserAgent,
                ct);

            if (response.IsInvalidCredentials)
            {
                var getChangeEmailRequest = new GetChangePhoneRequest
                {
                    OldPhone = request.OldPhone,
                    NewPhone = request.NewPhone,
                    IsInvalidCredentials = true
                };

                return RedirectToAction("ChangePhone", getChangeEmailRequest);
            }

            var getVerifyPhoneRequest = new GetVerifyPhoneRequest
            {
                TokenId = response.TokenId,
                IsInvalidCode = false
            };

            return RedirectToAction("VerifyPhone", getVerifyPhoneRequest);
        }

        [HttpGet("VerifyPhone")]
        public ActionResult VerifyPhone(GetVerifyPhoneRequest request)
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
            var isVerified = await _phoneVerificationService.VerifyAsync(request.TokenId, request.Code, ct);
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

        [HttpGet("ChangePassword")]
        public ActionResult ChangePassword(GetChangePasswordRequest request)
        {
            var model = new ChangePasswordViewModel(
                request.Login,
                request.IsPasswordsNotEqual,
                request.IsInvalidCredentials);

            return View("~/OAuth/Views/ChangePassword.cshtml", model);
        }

        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([FromForm] PostChangePasswordRequest request,
            CancellationToken ct)
        {
            if (request.NewPassword != request.NewPasswordConfirmation)
            {
                var getChangePasswordRequest = new GetChangePasswordRequest
                {
                    Login = request.Login,
                    IsPasswordsNotEqual = true
                };

                return RedirectToAction("ChangePassword", getChangePasswordRequest);
            }

            var response = await _passwordChangeService.ChangeAsync(
                request.Country,
                request.Login,
                request.OldPassword,
                request.NewPassword,
                ct);

            if (response.IsInvalidCredentials)
            {
                var getChangePasswordRequest = new GetChangePasswordRequest
                {
                    Login = request.Login,
                    IsInvalidCredentials = true
                };

                return RedirectToAction("ChangePassword", getChangePasswordRequest);
            }

            return View("~/OAuth/Views/PasswordChanged.cshtml");
        }

        [HttpGet("ResetPassword")]
        public ActionResult ResetPassword(GetResetPasswordRequest request)
        {
            var model = new ResetPasswordViewModel(request.Login, request.IsInvalidLogin);

            return View("~/OAuth/Views/ResetPassword.cshtml", model);
        }

        [HttpPost("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword([FromForm] PostResetPasswordRequest request, CancellationToken ct)
        {
            var response = await _passwordResetService.SendResetMessageAsync(
                request.Country,
                request.Login,
                IpAddress,
                UserAgent,
                ct);

            if (response.IsInvalidLogin)
            {
                var getResetPasswordRequest = new GetResetPasswordRequest
                {
                    Login = request.Login,
                    IsInvalidLogin = true
                };

                return RedirectToAction("ResetPassword", getResetPasswordRequest);
            }

            return View("~/OAuth/Views/ResetPasswordConfirmation.cshtml");
        }

        [HttpGet("ResetPasswordConfirmation")]
        public async Task<ActionResult> ResetPasswordConfirmation(
            ResetPasswordConfirmationRequest request,
            CancellationToken ct)
        {
            var isTokenValid = await _passwordResetService.IsTokenValidAsync(request.TokenId, request.Code, ct);
            if (!isTokenValid)
            {
                return BadRequest("Invalid code");
            }

            var model = new ResetPasswordConfirmationViewModel(
                request.TokenId,
                request.Code,
                request.IsPasswordsNotEqual);

            return View("~/OAuth/Views/SetNewPassword.cshtml", model);
        }

        [HttpPost("ResetPasswordConfirmation")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordConfirmation(
            [FromForm] PostResetPasswordConfirmationRequest request,
            CancellationToken ct)
        {
            if (request.NewPassword != request.NewPasswordConfirmation)
            {
                var resetPasswordConfirmationRequest = new ResetPasswordConfirmationRequest
                {
                    TokenId = request.TokenId,
                    Code = request.Code,
                    IsPasswordsNotEqual = true
                };

                return RedirectToAction("ResetPasswordConfirmation", resetPasswordConfirmationRequest);
            }

            var response = await _passwordResetService.SetNewPasswordAsync(
                request.TokenId,
                request.Code,
                request.NewPassword,
                ct);

            if (response.IsInvalidToken)
            {
                return BadRequest("Invalid code");
            }

            return View("~/OAuth/Views/NewPasswordSet.cshtml");
        }
    }
}