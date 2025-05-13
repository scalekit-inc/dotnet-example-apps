<p align="left">
  <a href="https://scalekit.com" target="_blank" rel="noopener noreferrer">
    <picture>
      <img src="https://cdn.scalekit.cloud/v1/scalekit-logo-dark.svg" height="64">
    </picture>
  </a>
  <br/>
</p>

# Official .NET SDK
<a href="https://scalekit.com" target="_blank" rel="noopener noreferrer">Scalekit</a> is an Enterprise Authentication Platform purpose built for B2B applications. This .NET SDK helps implement Enterprise Capabilities like Single Sign-on via SAML or OIDC in your .NET applications within a few hours.

<div>
📚 <a target="_blank" href="https://docs.scalekit.com">Documentation</a> - 🚀 <a target="_blank" href="https://docs.scalekit.com">Quick-start Guide</a> - 💻 <a target="_blank" href="https://docs.scalekit.com/apis">API Reference</a>
</div>
<hr />

## Pre-requisites

1. [Sign up](https://scalekit.com) for a Scalekit account.
2. Get your ```env_url```, ```client_id``` and ```client_secret``` from the Scalekit dashboard.

## Installation
Add Scalekit SDK package from Nuget package manager. 

```sh
dotnet add package Scalekit.SDK

```

## Usage

Initialize the Scalekit client using the appropriate credentials. Refer code sample below.
```.net
using Scalekit.SDK;
using Scalekit.SDK.Models;

ScalekitClient scalekitClient = new ScalekitClient(
    Environment.GetEnvironmentVariable("SCALEKIT_ENV_URL"),
    Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_ID"),
    Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_SECRET")
);
```

##### Minimum Requirements

The Scalekit .NET SDK is designed to operate with the following environment:

| Component | Version |
| --------- | ------- |
| .NET      | 8.0      |


## Examples - SSO with ASP.Net Core Web API 

Below is a simple code sample that showcases how to implement Single Sign-on using Scalekit SDK

```.net
[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ScalekitClient _scalekitClient;
    private readonly string? _redirectUri;

    public AuthController()
    {
        _scalekitClient = new ScalekitClient(scalekitUrl, clientId, clientSecret);
        _redirect_uri = "http://localhost:8000/auth/callback"
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

        string authUrl = _scalekitClient.GetAuthorizationUrl(_redirectUri, options);
        return Ok(new { url = authUrl });
    }


    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        string? code, 
        string? error_description = null, 
        string? idp_initiated_login = null)
    {
        try
        {
            var result = await _scalekitClient.AuthenticateWithCode(code, _redirectUri, new AuthenticationOptions());
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
}
```

## Example Apps

Fully functional sample applications written using Asp.Net Core Web API and Scalekit SDK. Feel free to clone the repo and run them locally.

- [ASP.Net Core Web API ](https://github.com/scalekit-inc/scalekit-dotnet-example)


## API Reference

Refer to our [API reference docs](https://docs.scalekit.com/apis) for detailed information about all our API endpoints and their usage.

## More Information

- Quickstart Guide to implement Single Sign-on in your application: [SSO Quickstart Guide](https://docs.scalekit.com)
- Understand Single Sign-on basics: [SSO Basics](https://docs.scalekit.com/best-practices/single-sign-on)
