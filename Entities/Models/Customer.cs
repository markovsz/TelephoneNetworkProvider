using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Customer
    {
        [Required]
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public User user { get; set; }


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
        [MaxLength(60)]
        public string Address { get; set; }


        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime RegistrationDate { get; set; }


        [Required]
        public bool IsBlocked { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime LastBlockTime { get; set; }


        [Required]
        public bool IsPhoneNumberHided { get; set; }
        

        [Required]
        public Decimal MoneyBalance { get; set; }

        public ICollection<Call> InitiatedCalls { get; set; }
        public ICollection<Call> ReceivedCalls { get; set; }
    }
}
