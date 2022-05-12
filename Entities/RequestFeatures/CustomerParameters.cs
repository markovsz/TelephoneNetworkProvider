using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CustomerParameters : RequestParameters
    {
        [MaxLength(20)]
        public string NamePart { get; set; }


        [MaxLength(20)]
        public string SurnamePart { get; set; }


        [MaxLength(20)]
        public string PatronymicPart { get; set; }


        [MaxLength(20)]
        public string PhoneNumberPart { get; set; }
    }
}
