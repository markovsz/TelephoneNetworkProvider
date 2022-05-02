﻿using System;
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
        public uint Id { get; set; }


        [Required]
        public uint CallerId { get; set; }


        [ForeignKey("CallerId")]
        public Customer Caller { get; set; }


        [Required]
        public uint CalledById { get; set; }


        [ForeignKey("CalledById")]
        public Customer CalledBy { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallInitiationTime { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallEndTime { get; set; }
    }
}
