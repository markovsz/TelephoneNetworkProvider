using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repository;
using BussinessLogic.Exceptions;

namespace BussinessLogic
{
    public class UserManipulationLogic : IUserManipulationLogic
    {
        private UserManager<User> _userManager;

        public UserManipulationLogic(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> CreateUser(string login, string password, string role)
        {
            //TODO
            User user = new User(login);
            user.UserName = login;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new InvalidUserDataException($"{error.Code} + {error.Description}");
            }

            result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new InvalidUserDataException($"{error.Code} + {error.Description}");
            }
            return user;
        }

        public async Task DeleteUser(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new InvalidUserDataException($"{error.Code} + {error.Description}");
            }
        }

        public async Task UpdateUser(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new InvalidUserDataException($"{error.Code} + {error.Description}");
            }
        }
    }
}
