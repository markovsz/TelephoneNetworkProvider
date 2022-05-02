using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class CustomerForRead
    {
        [Required]
        [Key]
        [MaxLength(20)]
        public string UserId { get; set; }


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
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime RegistrationDate { get; set; }


        [Required]
        public bool IsPhoneNumberHided { get; set; }
    }
}
