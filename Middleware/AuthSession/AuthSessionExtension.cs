using ASP_P42.Middleware.AuthSession;

namespace ASPNETITSTEP.Middleware.AuthSession
{
    // дотримання традицій ASP - оголошення розширення з Use... префіксом
    public static class AuthSessionExtension
    {
        public static IApplicationBuilder UseAuthSession(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}