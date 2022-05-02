using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CustomerForUpdateInAdministratorDto// : CustomerForUpdate
    {
        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [Required]
        public bool IsBlocked { get; set; }
    }
}
