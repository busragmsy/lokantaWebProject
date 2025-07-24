namespace lokantaWebProject.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Örn: Tatlılar, Çorbalar, Ana Yemekler


        // Navigation Property
        public List<MenuItem> MenuItems { get; set; }

    }
}
