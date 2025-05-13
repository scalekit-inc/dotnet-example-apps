<p align="center">
  <a href="https://scalekit.com" target="_blank" rel="noopener noreferrer">
    <picture>
      <img src="https://cdn.scalekit.cloud/v1/scalekit-logo-dark.svg" height="64">
    </picture>
  </a>
  <br/>
</p>
<h1 align="center">
  Scalekit ASP.NET Example App
</h1>

<h4 align="center">
Scalekit helps you ship Enterprise Auth in days.

This ASP.NET Core Web API Sample App showcases the Scalekit Official .NET SDK implementation.
</h4>

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Getting Started

1. [Sign up](https://scalekit.com) for a Scalekit account
2. Get your `env_url`, `client_id` and `client_secret` from the Scalekit dashboard

## Project Setup

1. Clone the repository:
```sh
git clone https://github.com/scalekit-inc/scalekit-net-example.git
cd scalekit-net-example
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
git clone --recursive https://github.com/scalekit-inc/scalekit-dotnet-example.git
# Build the ReactJS submodule
npm run build

# Copy all files from React build folder to ASP.NET Core WebAPI wwwroot folder
cp -r ./path-to-react-app/build/* ./path-to-aspnetcore-app/wwwroot/
```

4. Set up environment variables:
```sh
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

## Additional Resources
See the [Scalekit API docs](https://docs.scalekit.com) for more information about the API and authentication.