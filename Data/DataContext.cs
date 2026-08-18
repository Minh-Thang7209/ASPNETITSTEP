using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.UserData> UsersData { get; set; }
        public DbSet<Entities.UserRole> UsersRoles { get; set; }
        public DbSet<Entities.UserAccess> UserAccesses { get; set; }

        // Конструювання контексту налаштовується з Program.cs
        // відповідно, на час проєктування делегується конструктор
        // з параметрами підключенням.
        public DataContext (DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування, що виконуються під час першого завантаження 
            // контексту даних, зокрема, звʼязками між таблицями, унікальність тощо
            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(ua => ua.Login)
                .IsUnique();
        }
    }
}

/*
Entity Framework - інструмент спрощеної роботи з БД
Надає засоби для уніфікації - для роботи з БД вживаються
команди основної мови проєкту, однакові (або еквалентні) для різних СУБД.

Для підключення EF додаємо пакети NuGet:
- загальні інтерфейси
- їх імплементація під конкретну БД
- інструментарій командного рядка (зокрема, міграції)

Створюємо директорію (шар) проєкту Data
Entities - відбивають структуру таблиць БД. Для частини "Users":

[UsersData]        [UserAccesses]     [UserRoles]
[Id]      ---\     [Id]          /--- [Id]
[FullName]    \----[UsedId]     /     [Name]
[Email]            [RoleId] ---/      [CreateLevel]
[Phone]            [Login]            [ReadLevel]
[Birthdate]        [Salt]             [UpdateLevel]
[RegisteredAt]     [Dk]               [DeleteLevel]
[DeletedAt]
*/