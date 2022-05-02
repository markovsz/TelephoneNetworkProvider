using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AdministratorRepository
{
    public interface IAdministratorManager
    {
        ICustomerRepositoryForAdministrator Customers { get; }
        ICallRepositoryForAdministrator Calls { get; }
        IAdministratorMessageRepositoryForAdministrator Messages { get; }
        void Save();
    }
}
