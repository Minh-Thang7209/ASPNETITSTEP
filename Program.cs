using ASPNETITSTEP.Data;
using ASPNETITSTEP.Services.Hash;
using ASPNETITSTEP.Services.Kdf;
using ASPNETITSTEP.Services.Time;
using ASPNETITSTEP.Services.Storage;
using Microsoft.EntityFrameworkCore;
using ASPNETITSTEP.Middleware.AuthSession;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHash();
builder.Services.AddTime();
builder.Services.AddKdf();
builder.Services.AddStorage();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<DataAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => 
        policy
        .AllowAnyOrigin()   // відкритий АРІ - для всіх споживачів
        .AllowAnyHeader()   // дозволяємо усі заголовки
        .AllowAnyMethod()   // та усі методи запиту
                            // .WithMethods("GET", "POST") - якщо обмежуємо
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapStaticAssets();
app.UseSession();
app.UseAuthSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
