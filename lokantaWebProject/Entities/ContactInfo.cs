using System.ComponentModel.DataAnnotations;

namespace lokantaWebProject.Entities
{
    public class ContactInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Adres boş bırakılamaz.")]
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon numarası boş bırakılamaz.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta adresi en fazla 100 karakter olabilir.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL adresi giriniz.")]
        [StringLength(200, ErrorMessage = "Facebook URL'si en fazla 200 karakter olabilir.")]
        [Display(Name = "Facebook URL")]
        public string FacebookUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL adresi giriniz.")]
        [StringLength(200, ErrorMessage = "Instagram URL'si en fazla 200 karakter olabilir.")]
        [Display(Name = "Instagram URL")]
        public string InstagramUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL adresi giriniz.")]
        [StringLength(200, ErrorMessage = "Twitter URL'si en fazla 200 karakter olabilir.")]
        [Display(Name = "Twitter URL")]
        public string TwitterUrl { get; set; }
    }
}