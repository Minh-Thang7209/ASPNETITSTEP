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


/*
REST (Representational State Transfer) - це архітектурний стиль для розробки 
сервісів. Це набір принципів та обмежень щодо 
а) структури проєкту
б) взаємодії між клієнтом та сервером

- Stateless - відсутність стану на сервері (немає збереження даних) - немає 
 сесії всі дані, що мають бути повʼязані з користувачем, перекладаються з
 серверної сесії на клієнтський бік (до запитів). Це призводить до появи
 стандартів токенів, що передають зазнчені дані.
- Cachable - кешування даних на стороні клієнта (браузера) - відповідь 
 сервера має містити інформацію про те, чи можна кешувати дані, чи ні.
- Layered - не повинно бути однозначної можливості визначити чи підключення 
 відбувається безпосередньо до сервера, чи через проміжий сервер (проксі).
[client] <-------> [proxy] <-------> [server]
GET /item/123    GET /item/123 -------> 404 Not Found
                 помилка запиту
        <-------- 500 Internal Server Error 

        заголовки відповіді 
                Server: nginx          Server: Kestrel
                Date: 1234567891       Date: 1234567890
Server: ?
Data: ?
======== Дана вимога потребує перегляду принципів формування відповіей.
- Uniform interface - уніфікований інтерфейс взаємодії між клієнтом та сервером.
 Всі запити клієнта та відповіді сервера мають бути уніфіковані, тобто мати одну структура.
= Resource identification in requests - адреса запиту має вказувати на ресурс,
= Resource manipulation through - відповідь повинна містити мета дані, які зазначають можливості
 маніпаляцій з ресурсом (наприклад, GET, POST, PUT, DELETE)
= Self-descriptive - відповідь повинна містити інформацію про повідомлення 
 (про типи даних, що передаються)
= Hypermedia as the engine of application state (HATEOAS) - відповідь повинна містити
 посилання на внутрішні ресурси, якщо такі є (показувати свій зміст).
--------------------------------------------
GET /item/123:       
Гірший варіант (без статуса і метаданих):
{ 
    "id": 123,
    "name": "item name"
}
Кращий варіант:
{
    status: {   -- дублювання статусу потрібне для відокремлення статусу HTTP та статусу обробки даних
        "isOk": false, -- цілком нормально коли HTTP статус 200, тобто всі ланки передачі даних спрацювали,
        "code": 404, -- але обробка даних на сервері може бути неуспішною (наприклад,)
        "message": "Not found"
    },
    meta: {
        "cache": 86400, -- час дозволеного кешування відповіді (у секундах - доба)
        "service": "Price" -- назва сервіру (Resource identification)
        "manipulations": ["GET", "POST", "PATCH", "DELETE"] -- можливі манціпуляції
        "serverTime": 1234567890, -- час на сервері (Unix timestamp)
        "dataType": json/object -- тип даних, що є тілом відповіді (data)
        "links": {  --посилання на внутрішні ресурси (HATEOAS)
            "parent": "/items",
            "self": "/item/123/details",
            "delete": /item/123",
            "recipes": /item/123/recipes,
            "datasheet": /item/123/manual
        }
    -- для даних, що є масивами, додається інформація про пагінацію
        "pagination": {
            "page": 1,
            "pageSize": 10,
            "totalPage": 5,
            "totalItems": 50
        }
    -- позитивний момент - додавати дані про параметри, що можливі для запиту 
        "parameters": {
            "id": {"type": "string", value: "123"},
            "sort": {"type": "string", value: "asc"},
            "region": {"type": "string", value: "Odesa"}
        }
    -- якщо підтримується локалізація, то додається інформація про мову відповіді
        "locale": "en_US"
    },
    data: { "id": 123, "name": "item name"}
}

Семантика методів запиту:
"GET" - Read - оджержання даних без будь-яких змін в ресурсі,
"POST" - Create - створення нових даних, додавання сутностей у ресурс,
"PUT" - Replace - заміна (повне оновлення) сутності ,
"PATCH" - Update - часткове оновлення даних сутності,
"DELETE" - Delete - видалення  даних з ресурсу 
*/