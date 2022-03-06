using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Call
    {
        public Guid Id { get; set; }
        public Guid CallerId { get; set; }
        public Customer Caller { get; set; }
        public Guid CalledById { get; set; }
        public Customer CalledBy { get; set; }
        public DateTime CallInitiationTime { get; set; }
        public DateTime CallEndTime { get; set; }
    }
}
