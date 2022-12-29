using DomainLayer.Dto;
using DomainLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Contract
{
    public interface IUser
    {
        public List<User> GetAllUsers();
        public User GetUser(string id);
        public User GetUserByUsername(string username);
        public void PostPicture(PostDbDto post);
        public List<Post> GetAllPosts(int id);
        public int PostsCount(int id);
        public int FollowersCount(int id);
        public int FollowingsCount(int id);
        public Task<List<User>> SearchResult(string searchingString);
        public void UpdateUser(AccountDbDto account);
        public void DeleteUser(int id);
    }
}
