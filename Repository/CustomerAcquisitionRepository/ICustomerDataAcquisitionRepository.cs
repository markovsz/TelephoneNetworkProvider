using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerAcquisitionRepository
{
    public interface ICustomerDataAcquisitionRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters);
        Task<Customer> GetCustomerInfoAsync(int customerId, bool trackChanges);
    }
}
