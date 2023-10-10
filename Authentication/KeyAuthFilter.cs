using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace sti_sys_backend.Authentication;

public class KeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public KeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(Constant.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key Missing");
            return;
        }
        var apiKey = _configuration.GetValue<string>(Constant.ApiKeySectionName);
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
            return;
        }
    }
}