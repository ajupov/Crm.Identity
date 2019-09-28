using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crm.Identity.OAuth.Attributes.Security
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!(context.Result is ViewResult))
            {
                return;
            }

            var headers = context.HttpContext.Response.Headers;

            const string xContentTypeOptions = "X-Content-Type-Options";
            const string nosniff = "nosniff";
            if (!headers.ContainsKey(xContentTypeOptions))
            {
                headers.Add(xContentTypeOptions, nosniff);
            }

            const string xFrameOptions = "X-Frame-Options";
            const string sameorigin = "SAMEORIGIN";
            if (!headers.ContainsKey(xFrameOptions))
            {
                headers.Add(xFrameOptions, sameorigin);
            }

            const string csp = "default-src 'self';";
            const string contentSecurityPolicy = "Content-Security-Policy";
            if (!headers.ContainsKey(contentSecurityPolicy))
            {
                headers.Add(contentSecurityPolicy, csp);
            }

            const string xContentSecurityPolicy = "X-Content-Security-Policy";
            if (!headers.ContainsKey(xContentSecurityPolicy))
            {
                headers.Add(xContentSecurityPolicy, csp);
            }
        }
    }
}