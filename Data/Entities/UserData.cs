namespace ASPNETITSTEP.Data.Entities
{
    public class UserData
    {
        public Guid Id { get; set; }
        public String FullName { get; set;} = null!;
        public String Email { get; set; } = null!;
        public String? Phone { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}