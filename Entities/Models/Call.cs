using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Call
    {
        public uint Id { get; set; }
        public string CallerId { get; set; }
        public Customer Caller { get; set; }
        public string CalledById { get; set; }
        public Customer CalledBy { get; set; }
        public DateTime CallInitiationTime { get; set; }
        public DateTime CallEndTime { get; set; }
    }
}
