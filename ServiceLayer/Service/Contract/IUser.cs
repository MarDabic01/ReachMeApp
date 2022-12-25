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
    }
}
