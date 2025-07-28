using System;
using System.ComponentModel.DataAnnotations; // Doğrulama öznitelikleri için

namespace lokantaWebProject.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Gönderen Adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Gönderen Adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Gönderen Adı")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta adresi en fazla 100 karakter olabilir.")]
        [Display(Name = "Gönderen E-posta")]
        public string SenderEmail { get; set; }

        [Required(ErrorMessage = "Konu boş bırakılamaz.")]
        [StringLength(200, ErrorMessage = "Konu en fazla 200 karakter olabilir.")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj içeriği boş bırakılamaz.")]
        [StringLength(1000, ErrorMessage = "Mesaj içeriği en fazla 1000 karakter olabilir.")]
        [Display(Name = "Mesaj İçeriği")]
        public string MessageContent { get; set; }

        [Display(Name = "Gönderilme Tarihi")]
        public DateTime SentDate { get; set; } = DateTime.Now; // Mesajın gönderildiği tarih ve saat

        [Display(Name = "Okundu Bilgisi")]
        public bool IsRead { get; set; } = false; // Mesajın okunup okunmadığı bilgisi
    }
}