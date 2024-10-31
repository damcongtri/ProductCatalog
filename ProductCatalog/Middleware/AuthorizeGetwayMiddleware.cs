
using ProductCatalog.Attributes;

namespace ProductCatalog.Middleware
{
    public class AuthorizeGetwayMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var userId = context.Request.Headers["userId"];
            var roleName = context.Request.Headers["roleName"].ToString();

            var endpoint = context.GetEndpoint();
            var m_authorize = endpoint?.Metadata
                .GetMetadata<AuthorizeGetwayAttribute>();
            if (m_authorize.Roles.Contains(roleName))
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Unauthorized");
            }

            return next(context);
        }
    }
}
