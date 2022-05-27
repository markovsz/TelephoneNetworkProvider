using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.DataTransferObjects;

namespace BussinessLogic
{
    public interface IGuestLogic
    {
        Task<IEnumerable<CustomerForReadInGuestDto>> GetCustomersInfoAsync(CustomerParameters parameters);
        Task<CustomerForReadInGuestDto> GetCustomerInfoAsync(int customerId);
    }
}
