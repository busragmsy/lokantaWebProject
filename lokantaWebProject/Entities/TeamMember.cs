namespace lokantaWebProject.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }  // Örn: Baş Aşçı
        public string Bio { get; set; }
        public string PhotoUrl { get; set; }

    }
}
