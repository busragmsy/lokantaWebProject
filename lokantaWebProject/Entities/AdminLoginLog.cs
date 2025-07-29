using System.ComponentModel.DataAnnotations;

public class AdminLoginLog
{
    [Key]
    public int Id { get; set; }

    public string? UserName { get; set; }

    public DateTime LoginTime { get; set; } = DateTime.Now;

    public bool IsSuccessful { get; set; } // ✔ Başarılı mı

    public string? IpAddress { get; set; }  // ✔ IP bilgisi

    public string? UserAgent { get; set; }  // ✔ Tarayıcı bilgisi
}
