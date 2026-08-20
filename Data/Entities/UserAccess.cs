namespace ASPNETITSTEP.Data.Entities
{
    public class UserAccess
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public String Login { get; set; } = null!;
        public String Salt { get; set; } = null!;
        // derived key by RFC 2898
        public String Dk { get; set; } = null!;
        // навігаційні властивості - посилання на ініші сутності 
        public UserData UserData { get; set; } = null!;
        public UserRole UserRole { get; set; } = null!;
        // інверсіні навігаційні властивості - "зворотній бік" навігаційних властивостей 
        
    }
}