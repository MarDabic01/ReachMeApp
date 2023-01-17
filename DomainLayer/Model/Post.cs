using System;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Model
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Descrption { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImageData { get; set; }
        public int Likes { get; set; }
        public string LikedBy { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
