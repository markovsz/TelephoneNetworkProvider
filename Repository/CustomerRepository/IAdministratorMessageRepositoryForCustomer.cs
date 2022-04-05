using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public interface IAdministratorMessageRepositoryForCustomer
    {
        IEnumerable<AdministratorMessage> GetMessages(string userId, AdministratorMessageParameters parameters);
    }
}
