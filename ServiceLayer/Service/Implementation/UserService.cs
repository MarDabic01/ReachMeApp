using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Implementation
{
    public class UserService : IUser
    {
        private readonly DataContext context;

        public UserService(DataContext context)
        {
            this.context = context;
        }

        public List<User> GetAllUsers() => context.Users.ToList();
        public User GetUser(string id) => context.Users.FirstOrDefault(u => u.Id.ToString() == id);
        public User GetUserByUsername(string username) => context.Users.FirstOrDefault(u => u.Username == username);
        public void PostPicture(PostDbDto post)
        {
            Post newPost = new Post
            {
                Descrption = post.Description,
                UploadDate = DateTime.Now,
                ImageData = post.ImageData,
                Likes = 0,
                LikedBy = "",
                Comments = "",
                UserId = post.UserId
            };

            context.Posts.Add(newPost);
            context.SaveChanges();
        }

        public List<Post> GetAllPosts(int id) => context.Posts.Where(p => p.UserId == id).ToList();
        public int PostsCount(int id) => context.Posts.Where(p => p.UserId == id).ToList().Count;
        public int FollowersCount(int id) => context.Follows.Where(p => p.FollowingId == id).ToList().Count;
        public int FollowingsCount(int id) => context.Follows.Where(p => p.FollowerId == id).ToList().Count;
        public async Task<List<User>> SearchResult(string searchingString) => await context.Users.Where(u => u.Username.Contains(searchingString)).ToListAsync();
        public void UpdateUser(AccountDbDto account)
        {
            var updatedUser = context.Users.FirstOrDefault(u => u.Id == account.Id);

            updatedUser.Email = account.Email;
            updatedUser.Username = account.Username;
            updatedUser.Password = account.Password;
            updatedUser.ProfileBio = account.ProfileBio;
            updatedUser.ProfilePic = account.ProfilePic;
            context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            context.Users.Remove(user);

            List<Post> userPosts = context.Posts.Where(p => p.UserId == id).ToList();
            foreach (Post p in userPosts)
                context.Posts.Remove(p);

            context.SaveChanges();
        }
    }
}
