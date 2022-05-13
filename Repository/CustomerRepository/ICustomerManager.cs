using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CustomerRepository
{
    public interface ICustomerManager
    {
        ICustomerRepositoryForCustomer Customer { get; }
        ICallRepositoryForCustomer Calls { get; }
        IAdministratorMessageRepositoryForCustomer AdministratorMessages { get; }
        Task SaveAsync();
    }
}
