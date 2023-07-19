namespace ConsoleTestApp.EfCore.Entities
{
    public class ApplicationUserPhoto
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
