using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerAcquisitionRepository
{
    public interface ICustomerDataAcquisitionRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters);
        Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> expression, CustomerParameters parameters);
        Task<Customer> GetCustomerAsync(int customerId, bool trackChanges);
        Task<Customer> GetCustomerAsync(int customerId, Expression<Func<Customer, bool>> expression, bool trackChanges);
        Task<Customer> GetCustomerAsync(Expression<Func<Customer, bool>> expression, bool trackChanges);
    }
}
