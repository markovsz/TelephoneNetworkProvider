using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Call
    {
        [Required]
        [Key]
        public int Id { get; set; }

        //foreign keys for caller and called by are defined in repository context 
        public int CallerId { get; set; }

        public Customer Caller { get; set; }

        public int CalledById { get; set; }

        public Customer CalledBy { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallInitiationTime { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallEndTime { get; set; }
    }
}
