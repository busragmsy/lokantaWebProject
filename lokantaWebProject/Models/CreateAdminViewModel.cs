using System.ComponentModel.DataAnnotations;

namespace lokantaWebProject.Models
{
    public class CreateAdminViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        public string Role { get; set; } // "Admin", "SuperAdmin"
    }
}
