using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository.AdministratorRepository
{
    public class AdministratorMessageRepositoryForAdministrator : RepositoryBase<AdministratorMessage>, IAdministratorMessageRepositoryForAdministrator
    {

        public AdministratorMessageRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<AdministratorMessage>> GetMessageAsync(int id) =>
            await FindByCondition(m => m.Id.Equals(id), false)
            .ToListAsync();

        public async Task<IEnumerable<AdministratorMessage>> GetCustomerMessagesAsync(int customerId, AdministratorMessageParameters parameters) =>
            await FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .AdministratorMessageParametersHandler(parameters)
            .ToListAsync();

        public async Task<IEnumerable<AdministratorMessage>> GetCustomerWarningMessagesFromTimeAsync(int customerId, DateTime startTime) =>
            await FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .Where(m => m.Status.Equals("warning"))
            .Where(m => m.SendingTime.CompareTo(startTime) == 1)
            .ToListAsync();

        public async Task CreateMessageAsync(AdministratorMessage message) => await CreateAsync(message);

        public async Task DeleteMessageAsync(int id) =>
            Delete(await FindByCondition(m => m.Id.Equals(id), true).FirstOrDefaultAsync());
    }
}
