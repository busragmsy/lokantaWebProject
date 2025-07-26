using System.ComponentModel.DataAnnotations;

namespace lokantaWebProject.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olmalıdır.")]
        public string Name { get; set; } // Örn: Tatlılar, Çorbalar, Ana Yemekler

        // Navigation Property
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); // Null referans hatasını önlemek için başlatma
    }
}