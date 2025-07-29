using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace lokantaWebProject.Entities
{
    // IdentityUser<int> olarak değiştirin
    public class Admin : IdentityUser<int>
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")] 
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}