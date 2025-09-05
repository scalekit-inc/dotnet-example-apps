<p align="center">
  <a href="https://scalekit.com" target="_blank" rel="noopener noreferrer">
    <picture>
      <img src="https://cdn.scalekit.cloud/v1/scalekit-logo-dark.svg" height="64">
    </picture>
  </a>
  <br/>
</p>

<h1 align="center">
  Scalekit .NET Example Apps
</h1>

<p align="center">
  <strong>Auth stack for AI apps ⚡ Human auth capabilities</strong>
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/Scalekit.SDK"><img src="https://img.shields.io/nuget/v/Scalekit.SDK.svg" alt="NuGet Version"></a>
  <a href="https://github.com/scalekit-inc/dotnet-example-apps/blob/main/LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License: MIT"></a>
  <a href="https://docs.scalekit.com"><img src="https://img.shields.io/badge/docs-scalekit.com-blue" alt="Documentation"></a>
</p>

<p align="center">
  This ASP.NET Core Web API example demonstrates enterprise authentication using the official Scalekit .NET SDK
</p>

## 🚀 What This Example Shows

- **Enterprise SSO Integration**: Complete SAML/OIDC authentication flows
- **ASP.NET Core Integration**: Modern .NET web API patterns with dependency injection  
- **Session Management**: Secure token handling and user sessions
- **Production-Ready Code**: Error handling, validation, and security best practices

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Getting Started

1. [Sign up](https://scalekit.com) for a Scalekit account
2. Get your `env_url`, `client_id` and `client_secret` from the Scalekit dashboard

## Project Setup

1. Clone the repository:
```sh
# Clone the repository along with ReactJS submodule
git clone --recursive https://github.com/scalekit-developers/dotnet-example-apps.git
cd dotnet-example-apps
```

2. Install .NET dependencies:
```sh
# Install Scalekit.SDK package
dotnet add package Scalekit.SDK
dotnet add package DotNetEnv
dotnet restore
```

3. Add ReactJS submodule for frontend elements:
```sh
# Build the ReactJS submodule
cd web
npm run build

# Copy all files from React build folder to ASP.NET Core WebAPI wwwroot folder
cp -r ./path-to-react-app/build/* ./path-to-aspnetcore-app/wwwroot/
```

4. Set up environment variables:
```sh
# From the root directory
cp .env.example .env
```

Update `.env` with your Scalekit credentials:
```ini
SCALEKIT_ENV_URL=your_env_url
SCALEKIT_CLIENT_ID=your_client_id
SCALEKIT_CLIENT_SECRET=your_client_secret
```

5. Run the application in development mode:

```sh
# From the root directory
dotnet run

Open http://localhost:5125 to view it in the browser.
```

## Key Features

- **Enterprise SSO**: SAML 2.0 and OIDC protocols
- **User Management**: Create, update, and manage organization users
- **Directory Sync**: SCIM 2.0 for automated user provisioning
- **Admin Portal**: Embeddable admin interface for IT teams
- **Session Security**: JWT tokens with secure cookie management

## Additional Resources

- 📚 [Scalekit Documentation](https://docs.scalekit.com)
- 🔧 [API Reference](https://docs.scalekit.com/apis)
- 💬 [Community Support](https://github.com/scalekit-inc/scalekit-sdk-node-js/discussions)
- 🎯 [Get Started Guide](https://docs.scalekit.com/quick-start-guide)

---

<p align="center">
  Made with ❤️ by <a href="https://scalekit.com">Scalekit</a>
</p>