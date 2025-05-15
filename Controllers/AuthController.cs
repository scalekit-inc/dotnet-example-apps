using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Scalekit.SDK;
using Scalekit.SDK.Models;
using Scalekit.SDK.Logging;


using DotNetEnv;

namespace ScalekitSdkNet.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ScalekitClient _scalekitClient;

        private readonly ILogger<AuthController> _logger;
        private readonly string? _redirectUri;
        private static readonly Dictionary<string, User> _userStore = new();

        public AuthController(ILogger<AuthController> logger)
        {
            // Load .env file from the root directory
            Env.Load();
            _logger = logger;

            var adapterLogger = new MicrosoftLoggerAdapter<AuthController>(logger);
            var scalekitUrl = Environment.GetEnvironmentVariable("SCALEKIT_ENV_URL");
            var clientId = Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_SECRET");
            _redirectUri = Environment.GetEnvironmentVariable("AUTH_REDIRECT_URI");

            // Initialize ScalekitClient with env vars
            _scalekitClient = new ScalekitClient(scalekitUrl, clientId, clientSecret);
  
            // Uncomment the following line if you want to use the logger (Microsoft Adapter logger or a custom logger)
            // _scalekitClient = new ScalekitClient(scalekitUrl, clientId, clientSecret, adapterLogger);
            // _logger.LogInformation("AuthController initialized with ScalekitClient.");

        }
        
        [HttpGet("me")]
        public IActionResult GetUserProfile()
        {
            var userId = Request.Cookies["uid"];

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found." });

            var user = GetUserFromStore(userId);
            if (user == null)
                return Unauthorized(new { message = "User data not found." });
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var options = new AuthorizationUrlOptions
            {
                ConnectionId = request.ConnectionId,
                OrganizationId = request.OrganizationId,
                LoginHint = request.Email,
                State = Guid.NewGuid().ToString()
            };

            if (string.IsNullOrEmpty(_redirectUri))
            {
                return BadRequest(new { message = "Redirect URI is not configured." });
            }

            string authUrl = _scalekitClient.GetAuthorizationUrl(_redirectUri, options);
            return Ok(new { url = authUrl });
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            string? code, 
            string? error_description = null, 
            string? idp_initiated_login = null)
        {
            if (!string.IsNullOrEmpty(error_description))
                return BadRequest(new { message = error_description });

            if (string.IsNullOrEmpty(_redirectUri))
                return BadRequest(new { message = "Redirect URI is not configured." });

            if (!string.IsNullOrEmpty(idp_initiated_login))
                return await HandleIdpInitiatedLogin(idp_initiated_login, _redirectUri);

            if (string.IsNullOrEmpty(code))
                return BadRequest(new { message = "Authorization code not found." });

            return await HandleAuthorizationCodeFlow(code, _redirectUri);
        }

        private async Task<IActionResult> HandleIdpInitiatedLogin(string loginToken, string redirectUri)
        {
            try
            {
                var claims = await _scalekitClient.GetIdpInitiatedLoginClaims(loginToken);

                var options = new AuthorizationUrlOptions
                {
                    ConnectionId = claims.ConnectionId,
                    OrganizationId = claims.OrganizationId,
                    LoginHint = claims.LoginHint
                };

        
                var authUrl = _scalekitClient.GetAuthorizationUrl(redirectUri, options);
                return Redirect(authUrl.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private async Task<IActionResult> HandleAuthorizationCodeFlow(string code, string redirectUri)
        {
            try
            {
                var result = await _scalekitClient.AuthenticateWithCode(code, redirectUri, new AuthenticationOptions());
                var user = result.User;

                if (user == null || string.IsNullOrEmpty(user.Id))
                    return BadRequest(new { message = "User information is missing or invalid." });

                _userStore[user.Id] = new User
                {
                    Id = user.Id,
                    Name = $"{user.GivenName} {user.FamilyName}".Trim(),
                    Email = user.Email
                };

                Response.Cookies.Append("uid", user.Id, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                });

                return Redirect("/profile");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var userId = Request.Cookies["uid"];
            if (!string.IsNullOrEmpty(userId))
            {
                _userStore.Remove(userId);
            }
            Response.Cookies.Delete("uid");
            return Ok(new { message = "Logged out successfully." });
        }


        private object? GetUserFromStore(string userId)
        {
            if (_userStore.TryGetValue(userId, out var user))
            {
                return user;
            }
            return null;
        }
    }

    
    public class LoginRequest
    {
        public string? ConnectionId { get; set; }
        public string? OrganizationId { get; set; }
        public string? Email { get; set; }
    }
}
