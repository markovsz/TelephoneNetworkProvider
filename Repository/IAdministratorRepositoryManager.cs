using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAdministratorRepositoryManager
    {
        ICustomerRepositoryManager Customers { get; }
        ICustomerRepositoryManager Operators { get; }
        

        void Save();
    }
}
