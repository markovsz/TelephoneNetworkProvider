using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GuestRepository
{
    public interface IGuestManager
    {
        ICustomerRepositoryForGuest Customers { get; }
        Task SaveAsync();
    }
}
