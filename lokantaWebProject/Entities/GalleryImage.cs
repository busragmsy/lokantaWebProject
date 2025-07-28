using System.ComponentModel.DataAnnotations; // Doğrulama öznitelikleri için

namespace lokantaWebProject.Entities
{
    public class GalleryImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Görsel URL'si boş bırakılamaz.")] // Zorunlu alan
        [Url(ErrorMessage = "Geçerli bir URL adresi giriniz.")] // URL formatında olmalı
        [StringLength(500, ErrorMessage = "Görsel URL'si en fazla 500 karakter olabilir.")]
        [Display(Name = "Görsel URL'si")] // Görünümde kullanılacak etiket
        public string ImageUrl { get; set; }

        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        [Display(Name = "Açıklama (Caption)")] // Görünümde kullanılacak etiket
        public string Caption { get; set; } // Görsel açıklaması (isteğe bağlı)
    }
}