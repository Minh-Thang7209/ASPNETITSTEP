namespace ASPNETITSTEP.Services.Kdf
{
    public static class KdfServiceExtension
    {
        public static IServiceCollection AddKdf(
            this IServiceCollection services)
        {
            return services.AddSingleton<IKdfService, PbKdf1Service>();
        
        }
    }
}