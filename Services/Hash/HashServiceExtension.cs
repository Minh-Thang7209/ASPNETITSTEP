namespace ASPNETITSTEP.Services.Hash
{
    public static class HashServiceExtension
    {
        public static IServiceCollection AddHash(
            this IServiceCollection services)
        {
            return services.AddSingleton<IHashService, Md5HashService>();
            // return services.AddScoped<IHashService, Md5HashService>();
            // return services.AddTransient<IHashService, Md5HashService>();
        }
    }
}

// Сервіс можна зареєструвати 3 класичними спосабами 
