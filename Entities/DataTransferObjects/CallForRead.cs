using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class CallForRead
    {
        [Required]
        [Key]
        public uint Id { get; set; }


        [Required]
        public uint CallerId { get; set; }


        [Required]
        public uint CalledById { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallInitiationTime { get; set; }


        [Required]
        [Range(typeof(DateTime), "2020-01-01 00:00:00", "2999-12-31 23:59:59")]
        public DateTime CallEndTime { get; set; }
    }
}