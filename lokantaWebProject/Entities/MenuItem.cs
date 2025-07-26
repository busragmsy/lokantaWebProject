using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Column(TypeName) için
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // [ValidateNever] için

namespace lokantaWebProject.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yemek adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Yemek adı en fazla 100 karakter olmalıdır.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 10000.00, ErrorMessage = "Fiyat 0.01 ile 10000 arasında olmalıdır.")]
        [Column(TypeName = "decimal(18, 2)")] // Veritabanında ondalık hassasiyeti için
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Görsel URL'si zorunludur.")] // Görsel URL'sini zorunlu yaptık
        [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
        [StringLength(500, ErrorMessage = "Görsel URL'si en fazla 500 karakter olmalıdır.")]
        public string ImageUrl { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }

        // Navigation Property - Bu, formdan gelmediği için doğrulama dışı bırakılmalıdır.
        [ValidateNever]
        public Category Category { get; set; }
    }
}