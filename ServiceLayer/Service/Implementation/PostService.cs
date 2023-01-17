using DomainLayer.Model;
using Newtonsoft.Json;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Service.Implementation
{
    public class PostService : IPost
    {
        public Post selectedPost { get; set; }
        private readonly DataContext context;

        public PostService(DataContext context)
        {
            this.context = context;
        }

        public Post SelectPost(int postId)
        {
            selectedPost = context.Posts.FirstOrDefault(p => p.Id == postId);
            return selectedPost;
        }
        public User GetAuthor(int userId) => context.Users.FirstOrDefault(u => u.Id == userId);
        public void LikePost(int postId, int userId)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<string> likedBy = JsonConvert.DeserializeObject<List<string>>(post.LikedBy);
            likedBy.Add(userId.ToString());

            string json = JsonConvert.SerializeObject(likedBy);
            post.LikedBy = json;
            context.SaveChanges();
        }
        public void UnlikePost(int postId, int userId)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<string> likedBy = JsonConvert.DeserializeObject<List<string>>(post.LikedBy);
            likedBy.Remove(userId.ToString());

            string json = JsonConvert.SerializeObject(likedBy);
            post.LikedBy = json;
            context.SaveChanges();
        }
        public bool IsPostLiked(int postId, int userId)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<string> likedBy = JsonConvert.DeserializeObject<List<string>>(post.LikedBy);
            if (likedBy.Contains(userId.ToString()))
                return true;
            return false;
        }
        public int NumberOfLikes(int postId)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<string> likedBy = JsonConvert.DeserializeObject<List<string>>(post.LikedBy);
            return likedBy.Count;
        }
        public void PostComment(int postId, int userId, string comment)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(post.Comments);
            comments.Add(new Comment { UserId = userId, CommentString = comment });

            string json = JsonConvert.SerializeObject(comments);
            post.Comments = json;
            context.SaveChanges();
        }
        public List<Comment> GetComments(int postId)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == postId);
            List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(post.Comments);
            return comments;
        }
    }
}
