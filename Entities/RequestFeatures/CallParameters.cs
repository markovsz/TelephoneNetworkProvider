using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CallParameters
    {
        const int fixedPageSize = 50;
        public int PageNumber { get; set; }
        public string PhoneNumberPart { get; set; }
        public DateTime MinCallInitiationTime { get; set; }
        public DateTime MaxCallInitiationTime { get; set; }
        public DateTime MinCallEndTime { get; set; }
        public DateTime MaxCallEndTime { get; set; }

    }
}
