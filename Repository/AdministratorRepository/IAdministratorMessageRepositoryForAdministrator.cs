using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public interface IAdministratorMessageRepositoryForAdministrator
    {
        IEnumerable<AdministratorMessage> GetMessage(int id);
        IEnumerable<AdministratorMessage> GetCustomerMessages(int customerId, AdministratorMessageParameters parameters);
        IEnumerable<AdministratorMessage> GetCustomerWarningMessagesFromTime(int customerId, DateTime startTime);
        void CreateMessage(AdministratorMessage message);
        void DeleteMessage(int id);
        //IEnumerable<AdministratorMessage> GetAdministratorMessagesByCustomerId(int customerId, AdministratorMessageParameters parameters);

    }
}
