using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Price için gereklidir

namespace lokantaWebProject.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yemek adı zorunludur.")] // Yemek adının zorunlu olmasını sağlar
        [StringLength(100, ErrorMessage = "Yemek adı en fazla 100 karakter olmalıdır.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")] // Açıklamanın zorunlu olmasını sağlar
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")] // Fiyatın zorunlu olmasını sağlar
        [Range(0.01, 10000.00, ErrorMessage = "Fiyat 0.01 ile 10000 arasında olmalıdır.")] // Fiyat aralığı
        [Column(TypeName = "decimal(18, 2)")] // Veritabanında ondalık basamakları ayarlar
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL giriniz.")] // Geçerli bir URL formatı kontrolü
        [StringLength(500, ErrorMessage = "Görsel URL'si en fazla 500 karakter olmalıdır.")]
        public string? ImageUrl { get; set; } // Bu alanı hala null yapılabilir bırakıyorum, ancak siz zorunlu isterseniz [Required] ekleyebilirsiniz.

        // Foreign Key
        [Required(ErrorMessage = "Kategori seçimi zorunludur.")] // Kategori seçiminin zorunlu olmasını sağlar
        public int CategoryId { get; set; }

        // Navigation Property
        [ValidateNever]
        public Category Category { get; set; }
    }
}