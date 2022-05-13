using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository.CustomerRepository
{
    public class AdministratorMessageRepositoryForCustomer : RepositoryBase<AdministratorMessage>, IAdministratorMessageRepositoryForCustomer
    {
        public AdministratorMessageRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<AdministratorMessage>> GetMessagesAsync(int customerId, AdministratorMessageParameters parameters) =>
            await FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .AdministratorMessageParametersHandler(parameters)
            .ToListAsync();
    }
}
