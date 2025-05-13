using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Scalekit.SDK;

namespace Webhook.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly ScalekitClient _scaleClient;
        private readonly string _redirectUri;
        private readonly string _webhookSecret;

        public WebhookController()
        {
            // Load .env file from the root directory
            Env.Load();

            var scalekitUrl = Environment.GetEnvironmentVariable("SCALEKIT_ENV_URL");
            var clientId = Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("SCALEKIT_CLIENT_SECRET");
            _redirectUri = Environment.GetEnvironmentVariable("AUTH_REDIRECT_URI") ?? string.Empty;
            _webhookSecret = Environment.GetEnvironmentVariable("WEBHOOK_SECRET") ?? string.Empty;

            // Initialize ScalekitClient with env vars
            _scaleClient = new ScalekitClient(scalekitUrl, clientId, clientSecret);

            
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            try{
                // Read headers
                var headers = Request.Headers;

                var webhookId = headers.TryGetValue("webhook-id", out var id) ? id.ToString() : null;
                var webhookTimestamp = headers.TryGetValue("webhook-timestamp", out var ts) ? ts.ToString() : null;
                var webhookSignature = headers.TryGetValue("webhook-signature", out var sig) ? sig.ToString() : null;


                var headersDict = Request.Headers
                    .ToDictionary(kvp => kvp.Key.ToLowerInvariant(), kvp => kvp.Value.ToString());

                var body = await new StreamReader(Request.Body).ReadToEndAsync(); 

                // Process webhook data
                bool result = _scaleClient.VerifyWebhookPayload(_webhookSecret, headersDict, body);

                if (!result)
                {
                    return Unauthorized("Invalid webhook signature");
                }        
                return Ok("Webhook received");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred" + ex.Message);
            }
        }
    }
}
