using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;
using Entities.DataTransferObjects;

namespace BussinessLogic
{
    public interface ICustomerDataAcquisitionLogic<T> where T : CustomerForRead
    {
        IEnumerable<T> GetCustomers(CustomerParameters parameters);
        T GetCustomerInfo(int customerId);
    }
}
