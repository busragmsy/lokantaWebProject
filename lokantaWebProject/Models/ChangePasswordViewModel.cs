using System.ComponentModel.DataAnnotations;

namespace lokantaWebProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
