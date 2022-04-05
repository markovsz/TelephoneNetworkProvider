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
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
