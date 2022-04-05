using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CustomerForUpdateInAdministratorDto// : CustomerForUpdate
    {
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
    }
}
