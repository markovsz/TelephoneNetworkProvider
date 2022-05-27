using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.CustomerAcquisitionRepository
{
    public class CustomerDataAcquisitionRepository : RepositoryBase<Customer>, ICustomerDataAcquisitionRepository
    {
        public CustomerDataAcquisitionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Customer> GetCustomerAsync(int customerId, Expression<Func<Customer, bool>> expression, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(customerId), trackChanges)
                .Where(expression)
                .FirstOrDefaultAsync();

        public async Task<Customer> GetCustomerAsync(int customerId, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(customerId), trackChanges)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters) =>
            await FindAll(false)
                .CustomerParametersHandler(parameters)
                .ToListAsync();

        public async Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> expression, CustomerParameters parameters) =>
            await FindAll(false)
                .Where(expression)
                .CustomerParametersHandler(parameters)
                .ToListAsync();
    }
}
