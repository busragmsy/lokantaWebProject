namespace lokantaWebProject.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // şifreyi şifreli tut
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
