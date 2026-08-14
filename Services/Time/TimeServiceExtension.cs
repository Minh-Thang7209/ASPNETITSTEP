namespace ASPNETITSTEP.Services.Time
{
    public static class TimeServiceExtension
    {
        public static IServiceCollection AddTime(this IServiceCollection services)
        {
            services.AddSingleton<ITimeService, TimeService>();

            return services;
        }
    }
}