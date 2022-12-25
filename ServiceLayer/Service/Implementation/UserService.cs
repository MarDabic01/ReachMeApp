using DomainLayer.Model;
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
    }
}
