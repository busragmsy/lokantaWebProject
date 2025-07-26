using System.ComponentModel.DataAnnotations; // Doğrulama öznitelikleri için
using System.ComponentModel.DataAnnotations.Schema; // ColumnType için

namespace lokantaWebProject.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum yapan kişinin adı boş bırakılamaz.")] // Zorunlu alan
        [StringLength(100, ErrorMessage = "Yorum yapan kişinin adı en fazla 100 karakter olabilir.")] // Maksimum karakter uzunluğu
        [Display(Name = "Yorum Yapan Adı")] // Görünümde kullanılacak etiket
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Yorum içeriği boş bırakılamaz.")] // Zorunlu alan
        [StringLength(500, ErrorMessage = "Yorum içeriği en fazla 500 karakter olabilir.")] // Maksimum karakter uzunluğu
        [Display(Name = "Yorum İçeriği")] // Görünümde kullanılacak etiket
        public string Text { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL giriniz.")] // URL formatında olmalı
        [Display(Name = "Fotoğraf/İkon URL'si")] // Görünümde kullanılacak etiket
        public string ImageUrl { get; set; }

        [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")] // Değer aralığı
        [Display(Name = "Puan (Yıldız)")] // Görünümde kullanılacak etiket
        public int Rating { get; set; } = 5; // Varsayılan değer 5

        [Display(Name = "Eklenme Tarihi")] // Görünümde kullanılacak etiket
        public DateTime DatePosted { get; set; } = DateTime.Now; // Varsayılan olarak şimdiki zaman
    }
}