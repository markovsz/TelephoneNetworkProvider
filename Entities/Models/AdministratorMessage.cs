using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AdministratorMessage
    {
        [Required]
        [Key]
        public int Id { get; set; }


        [Required]
        public int CustomerId { get; set; }


        [Required]
        [ForeignKey("CustomerId")]
        public Customer customer { get; set; }


        [Required]
        [MaxLength(20)]
        public string Status { get; set; }//check enumeration of statuses


        [Required]
        [MaxLength(200)]
        public string Text { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime SendingTime { get; set; }
    }
}
