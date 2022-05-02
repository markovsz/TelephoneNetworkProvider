using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class CustomerForUpdate
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }


        [Required]
        [MaxLength(20)]
        public string Surname { get; set; }


        [Required]
        [MaxLength(20)]
        public string Patronymic { get; set; }


        [Required]
        [MaxLength(20)]
        public string Address { get; set; }


        [Required]
        public bool IsPhoneNumberHided { get; set; }
    }
}
