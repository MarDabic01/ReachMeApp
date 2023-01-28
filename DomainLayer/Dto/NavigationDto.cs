using DomainLayer.Model;

namespace DomainLayer.Dto
{
    public class NavigationDto
    {
        public NavigationDto()
        {

        }

        public NavigationDto(User currentUser)
        {
            CurrentUser = currentUser;
        }

        public string SearchString { get; set; }
        public User CurrentUser { get; set; }
    }
}
