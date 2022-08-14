using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware    
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiKeyAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _key;
        private readonly string _value;
        public ApiKeyAuthorize(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isApiKeyPresent = context.HttpContext.Request.Headers.TryGetValue(_key, out var extractedApiKey);
            if (isApiKeyPresent)  {
                if (!_value.Equals(extractedApiKey)) {
                    context.Result = new JsonResult(new { Message = "Api Key is invalid." })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            } else {
                context.Result = new JsonResult(new {Message = "Api Key has not been provided in the request headers."})
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
