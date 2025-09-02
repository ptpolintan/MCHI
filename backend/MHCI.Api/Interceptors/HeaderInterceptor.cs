using MHCI.Infrastructure.Stores;

namespace MHCI.Api.Interceptors
{
    public class HeaderInterceptor
    {
        private readonly RequestDelegate _next;

        public HeaderInterceptor(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("currentuserid", out var userIdValue)
                && int.TryParse(userIdValue, out var userId))
            {
                var user = UserStore.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    context.Items["currentuserid"] = user.Id;
                    context.Items["currentuserrole"] = user.Role;
                }
            }

            await _next(context);
        }
    }
}
