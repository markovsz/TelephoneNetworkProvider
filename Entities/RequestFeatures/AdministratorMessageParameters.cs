using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class AdministratorMessageParameters : RequestParameters
    {
        [MaxLength(20)]
        public string UserId { get; set; }


        [MaxLength(20)]
        public string Status { get; set; }
    }
}
