namespace lokantaWebProject.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }       // Yorum sahibinin adı
        public string Text { get; set; }             // Yorum içeriği
        public string ImageUrl { get; set; }         // Fotoğraf ya da ikon class'ı
        public int Rating { get; set; } = 5;         // 1-5 yıldız
        public DateTime DatePosted { get; set; } = DateTime.Now;  // Eklenme tarihi

    }
}
