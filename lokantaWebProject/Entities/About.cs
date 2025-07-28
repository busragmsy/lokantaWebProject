using System.ComponentModel.DataAnnotations; // Doğrulama öznitelikleri için

namespace lokantaWebProject.Entities
{
    public class About
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık boş bırakılamaz.")] // Zorunlu alan
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")] // Maksimum karakter uzunluğu
        [Display(Name = "Başlık")] // Görünümde kullanılacak etiket
        public string Title { get; set; } // Örn: "Vizyonumuz"

        [Required(ErrorMessage = "Açıklama boş bırakılamaz.")] // Zorunlu alan
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")] // Maksimum karakter uzunluğu
        [Display(Name = "Açıklama")] // Görünümde kullanılacak etiket
        public string Description { get; set; }
    }
}