using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Model
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public User User { get; set; }
    }
}
