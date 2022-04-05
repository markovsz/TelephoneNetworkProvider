using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CustomerParameters : RequestParameters
    {
        public string NamePart { get; set; }
        public string SurnamePart { get; set; }
        public string PatronymicPart { get; set; }
        public string PhoneNumberPart { get; set; }
    }
}
