using GameTrackerApi.GameAlerts;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace GameTrackerApi.TextingClient;

public class TwilioTextingClient : ITextingClient
{
    private readonly ILogger<TwilioTextingClient> _logger;
    private readonly IConfiguration _configuration;

    public TwilioTextingClient(ILogger<TwilioTextingClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public async Task SendTextAsync(string messageText)
    {
        InitializeTwilioClient();
        await CreateTwilioMessage(messageText);
    }

    private void InitializeTwilioClient()
    {
        string accountSid = _configuration["Twilio:AccountSid"] ?? string.Empty;
        string authToken = _configuration["Twilio:AuthToken"] ?? string.Empty;
        if (TwilioAuthInfoIsMissing(accountSid, authToken))
        {
            _logger.LogError("Twilio auth info is missing");
            return;
        }
        
        TwilioClient.Init(accountSid, authToken);
    }

    private bool TwilioAuthInfoIsMissing(string accountSid, string authToken)
    {
        return string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken);
    }


    private async Task CreateTwilioMessage(string messageText)
    {
        string toNumber = _configuration["Twilio:ToNumber"] ?? string.Empty;
        string fromNumber = _configuration["Twilio:FromNumber"] ?? string.Empty;
        if (PhoneNumbersForSmsAreMissing(toNumber, fromNumber))
        {
            _logger.LogError("Twilio to or from number is missing");
            return;
        }
        
        var message = await MessageResource.CreateAsync(
            body: messageText,
            from: new Twilio.Types.PhoneNumber("+18339641356"),
            to: new Twilio.Types.PhoneNumber(toNumber)
        );
        
        _logger.LogInformation("Sending text: {@Message}", message);
    }


    private static bool PhoneNumbersForSmsAreMissing(string toNumber, string fromNumber)
    {
        return string.IsNullOrEmpty(toNumber) || string.IsNullOrEmpty(fromNumber);
    }
}