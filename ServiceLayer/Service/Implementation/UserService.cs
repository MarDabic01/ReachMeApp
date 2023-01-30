using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Implementation
{
    public class UserService : IUser
    {
        private readonly DataContext context;
        private readonly IForgotPassword forgotPasswordService;

        public UserService(DataContext context, IForgotPassword forgotPasswordService)
        {
            this.context = context;
            this.forgotPasswordService = forgotPasswordService;
        }

        public List<User> GetAllUsers() => context.Users.ToList();
        public User GetUser(string id) => context.Users.FirstOrDefault(u => u.Id.ToString() == id);
        public User GetUserByUsername(string username) => context.Users.FirstOrDefault(u => u.Username == username);
        public void PostPicture(PostDto post)
        {
            Post newPost = new Post
            {
                Descrption = post.Description,
                UploadDate = DateTime.Now,
                ImageData = post.ImageData,
                Likes = 0,
                LikedBy = "[]",
                Comments = "[]",
                UserId = post.UserId
            };

            context.Posts.Add(newPost);
            context.SaveChanges();
        }

        public List<Post> GetAllPosts(int id) => context.Posts.Where(p => p.UserId == id)
            .OrderByDescending(d => d.UploadDate).ToList();

        public List<Post> GetAllFollowingPosts(int id)
        {
            List<Follow> followings = context.Follows.Where(f => f.FollowerId == id).ToList();
            List<Post> posts = new List<Post>();
            foreach (Follow f in followings)
            {
                posts.AddRange(context.Posts.Where(p => p.UserId == f.FollowingId).ToList());
            }
            return posts;
        }
        public int PostsCount(int id) => context.Posts.Where(p => p.UserId == id).ToList().Count;
        public int FollowersCount(int id) => context.Follows.Where(p => p.FollowingId == id).ToList().Count;
        public int FollowingsCount(int id) => context.Follows.Where(p => p.FollowerId == id).ToList().Count;
        public async Task<List<User>> SearchResult(string searchingString) => await context.Users.Where(u => u.Username.Contains(searchingString)).ToListAsync();
        public void UpdateUser(AccountDto account)
        {
            var updatedUser = context.Users.FirstOrDefault(u => u.Id == account.Id);

            updatedUser.Email = account.Email;
            updatedUser.Username = account.Username;
            updatedUser.Password = forgotPasswordService.EncryptString(account.Password);
            updatedUser.ProfileBio = account.ProfileBio;
            updatedUser.ProfilePic = account.ProfilePicData;
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

        public string ConvertImage(IFormFile imageFile)
        {
            byte[] bytes = null;
            if (imageFile != null)
            {
                using (Stream fs = imageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
            return null;
        }
    }
}
