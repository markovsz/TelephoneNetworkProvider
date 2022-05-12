using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace BussinessLogic
{
    public interface IUserManipulationLogic
    {
        Task<User> CreateUser(string login, string password, string role);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
    }
}
