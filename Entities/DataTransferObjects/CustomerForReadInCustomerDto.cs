using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CustomerForReadInCustomerDto : CustomerForRead
    {
        public string Address { get; set; }
        public bool IsBlocked { get; set; }
        public Decimal MoneyBalance { get; set; }
    }
}
