using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;

public class OAuthHelper
{
    private readonly IConfiguration _configuration;

    public OAuthHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public OAuthOptions GetOAuthOptions()
    {
        var oauthSettings = _configuration.GetSection("OAuthSettings");

        return new OAuthOptions
        {
            ClientId = oauthSettings["ClientId"],
            ClientSecret = oauthSettings["ClientSecret"],
            CallbackPath = oauthSettings["CallbackPath"],
            AuthorizationEndpoint = oauthSettings["AuthorizationEndpoint"],
            TokenEndpoint = oauthSettings["TokenEndpoint"],
            SaveTokens = true
        };
    }
}
