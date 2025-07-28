using System;
using Microsoft.AspNetCore.Identity;

namespace lokantaWebProject.Entities
{
    // IdentityUser<int> olarak değiştirin
    public class Admin : IdentityUser<int>
    {
        
        public string? FullName { get; set; } // Sadece eklemek istediğiniz alanlar kalsın
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        
    }
}