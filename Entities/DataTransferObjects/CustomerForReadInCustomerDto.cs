using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CustomerForReadInCustomerDto : CustomerForRead
    {
        [Required]
        [MaxLength(60)]
        public string Address { get; set; }


        [Required]
        public bool IsBlocked { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime LastBlockTime { get; set; }


        [Required]
        public Decimal MoneyBalance { get; set; }
    }
}
