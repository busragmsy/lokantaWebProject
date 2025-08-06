using lokantaWebProject.Entities;

namespace lokantaWebProject.Models
{
    public class HomePageViewModel
    {
        public AboutViewModel AboutSectionData { get; set; }
        public List<Category> MenuCategories { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }
        public List<Comment> Comments { get; set; }
        public ContactInfo ContactInfoData { get; set; }
        public Message Message { get; set; }
    }
}
