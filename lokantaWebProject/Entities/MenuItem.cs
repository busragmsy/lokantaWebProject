namespace lokantaWebProject.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } // Yemek görseli


        // Foreign Key
        public int CategoryId { get; set; }


        // Navigation Property
        public Category Category { get; set; }

    }
}
