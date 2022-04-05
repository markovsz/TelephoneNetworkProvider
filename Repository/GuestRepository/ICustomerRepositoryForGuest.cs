using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public interface ICustomerRepositoryForGuest
    {
        IEnumerable<Customer> GetCustomers(CustomerParameters parameters);
    }
}
