using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public class AdministratorMessageRepositoryForAdministrator : RepositoryBase<AdministratorMessage>, IAdministratorMessageRepositoryForAdministrator
    {

        public AdministratorMessageRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<AdministratorMessage> GetMessage(int id) =>
            FindByCondition(m => m.Id.Equals(id), false);

        public IEnumerable<AdministratorMessage> GetCustomerMessages(int customerId, AdministratorMessageParameters parameters) =>
            FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .AdministratorMessageParametersHandler(parameters)
            .ToList();

        public IEnumerable<AdministratorMessage> GetCustomerWarningMessagesFromTime(int customerId, DateTime startTime) =>
            FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .Where(m => m.Status.Equals("warning"))
            .Where(m => m.SendingTime.CompareTo(startTime) == 1)
            .ToList();

        public void CreateMessage(AdministratorMessage message) => Create(message);

        public void DeleteMessage(int id) =>
            Delete(FindByCondition(m => m.Id.Equals(id), true).FirstOrDefault());

        //public IEnumerable<AdministratorMessage> GetAdministratorMessagesByCustomerUserId(int customerId, AdministratorMessageParameters parameters) =>
        //    FindByCondition(m => m.CustomerId.Equals(customerId), false)
        //    .AdministratorMessageParametersHandler(parameters)
        //    .ToList();
    }
}
