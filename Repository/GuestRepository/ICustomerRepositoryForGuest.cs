using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.GuestRepository
{
    public interface ICustomerRepositoryForGuest
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters);
        Task<Customer> GetCustomerAsync(int customerId);
    }
}
