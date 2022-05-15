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
        Task<User> CreateUserAsync(string login, string password, string role);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
