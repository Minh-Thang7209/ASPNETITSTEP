namespace ASPNETITSTEP.Data.Entities
{
    public class AuthJournal
    {
         public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Login { get; set; } = null!;

        public string Dk { get; set; } = null!;

        public bool IsOk { get; set; }
    }
}