using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAdministratorManager
    {
        ICustomerRepositoryForAdministrator Customers { get; }
        ICallRepositoryForAdministrator Calls { get; }
        void Save();
    }
}
