namespace ASPNETITSTEP.Services.Storage
{
    public static class StorageServiceExtension
    {
        public static IServiceCollection AddStorage(
           this IServiceCollection services)
        {
            return services.AddSingleton<IStorageService, LocalStorageService>();
        }
    }
}