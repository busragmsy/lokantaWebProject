using System.ComponentModel.DataAnnotations;

namespace lokantaWebProject.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        public string Name { get; set; }  // Örn: Tatlılar, Çorbalar, Ana Yemekler

        // Navigation Property
        public List<MenuItem> MenuItems { get; set; }
    }
}
