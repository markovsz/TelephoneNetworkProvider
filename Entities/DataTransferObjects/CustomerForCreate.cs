using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class CustomerForCreate
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


        [MaxLength(60)]
        public string Address { get; set; }


        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [Required]
        public string IsPhoneNumberHidedStr { get; set; } //json convertation problem


        [Required]
        [MaxLength(20)]
        public string Login { get; set; }//UserId


        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
