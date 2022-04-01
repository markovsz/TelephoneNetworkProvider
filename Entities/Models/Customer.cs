using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Customer
    {
        //public uint Id { get; set; }
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public uint WarningMessagesBeforeLock { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsPhoneNumberHided { get; set; }
        public Decimal MoneyBalance { get; set; }
    }
}
