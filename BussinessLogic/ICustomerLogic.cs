using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Entities.Models;

namespace BussinessLogic
{
    public interface ICustomerLogic
    {
        void UpdateCustomerInfo(CustomerForUpdateInCustomerDto customerDto);
        void ReplenishTheBalance(Decimal currency);
        IEnumerable<Call> GetCalls(CallParameters parameters);
        CustomerForReadInCustomerDto GetCustomerInfo(string userId);
    }
}
