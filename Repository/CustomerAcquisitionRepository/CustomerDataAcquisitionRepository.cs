using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.CustomerAcquisitionRepository
{
    public class CustomerDataAcquisitionRepository : RepositoryBase<Customer>, ICustomerDataAcquisitionRepository
    {
        public CustomerDataAcquisitionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Customer> GetCustomerInfo(int customerId, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(customerId), trackChanges)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Customer>> GetCustomers(CustomerParameters parameters) =>
            await FindAll(false)
                .CustomerParametersHandler(parameters)
                .ToListAsync();
    }
}
