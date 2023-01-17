using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Contract
{
    public interface IUser
    {
        public List<User> GetAllUsers();
        public User GetUser(string id);
        public User GetUserByUsername(string username);
        public void PostPicture(PostDto post);
        public List<Post> GetAllPosts(int id);
        public List<Post> GetAllFollowingPosts(int id);
        public int PostsCount(int id);
        public int FollowersCount(int id);
        public int FollowingsCount(int id);
        public Task<List<User>> SearchResult(string searchingString);
        public void UpdateUser(AccountDto account);
        public void DeleteUser(int id);
        public string ConvertImage(IFormFile image);
    }
}
