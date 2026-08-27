using Microsoft.EntityFrameworkCore;

namespace ASPNETITSTEP.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.UserData> UsersData { get; set; }
        public DbSet<Entities.UserRole> UsersRoles { get; set; }
        public DbSet<Entities.UserAccess> UserAccesses { get; set; }
        public DbSet<Entities.AuthJournal> AuthJournals { get; set; }
        // Конструювання контексту налаштовується з Program.cs
        // відповідно, на час проєктування делегується конструктор
        // з параметрами підключенням.
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування, що виконуються під час першого завантаження 
            // контексту даних, зокрема, звʼязками між таблицями, унікальність тощо
            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(ua => ua.Login)
                .IsUnique();

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserData)
                .WithMany(ud => ud.Accesses)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserRole)
                .WithMany()
                .HasForeignKey(ua => ua.RoleId);

            modelBuilder.Entity<Entities.UserRole>()
                .HasData([
                    new() {
                        Id = Guid.Parse("ACB35324-7B84-4E3B-9A26-00AAD72A600C"),
                        Name = "Admin",
                        Description = "Кореневий адміністратор",
                        CreateLevel = 10,
                        ReadLevel = 10,
                        UpdateLevel = 10,
                        DeleteLevel = 10,
                    }
                ]);

            modelBuilder.Entity<Entities.UserRole>()
            .HasData([
                new() {
                        Id = Guid.Parse("702D05C2-FCD0-4C1D-B0BB-2AEB4B98F91A"),
                        Name = "User",
                        Description = "Самозаєстрований користувач",
                        CreateLevel = 0,
                        ReadLevel = 0,
                        UpdateLevel = 0,
                        DeleteLevel = 0,
                    }
            ]);
            modelBuilder.Entity<Entities.UserData>()
                .HasData([
                    new(){
                            Id = Guid.Parse("190052CA-F844-498A-A05F-1D4BA2ADC0E8"),
                            FullName = "Адміністратор системи",
                            BirthDate = DateTime.UnixEpoch,
                            Email = "CHANGE@ME",
                            Phone = "CHANGE_ME",
                            RegisteredAt = DateTime.UnixEpoch,
                        }
            ]);
            modelBuilder.Entity<Entities.UserAccess>()
                .HasData([
                    new(){
                            Id = Guid.Parse("96DCBBBA-9AEE-44A2-8835-72DFE4E1A710"),
                            UserId = Guid.Parse("190052CA-F844-498A-A05F-1D4BA2ADC0E8"),
                            RoleId = Guid.Parse("702D05C2-FCD0-4C1D-B0BB-2AEB4B98F91A"),
                            Login = "Admin",
                            Salt = "96DCBBBA-9AEE-44A2-8835-72DFE4E1A710",
                            Dk = "FCB57CECE720632FDBB68958CF953E46",
                        }
                ]);
            
            // сідування (англ. seed - зерно) - внесенення початкових даних, зокрема, базові ролі та кореневий адміністратор
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