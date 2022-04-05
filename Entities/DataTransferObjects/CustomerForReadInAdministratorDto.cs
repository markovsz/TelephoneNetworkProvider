using System;

namespace Entities.DataTransferObjects
{
    public class CustomerForReadInAdministratorDto : CustomerForRead
    {
        public string Address { get; set; }
        public bool IsBlocked { get; set; }
        public Decimal MoneyBalance { get; set; }
    }
}