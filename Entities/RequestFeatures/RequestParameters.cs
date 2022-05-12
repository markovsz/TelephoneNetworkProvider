using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RequestParameters
    {
        public const int MaxPageSize = 50;

        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, MaxPageSize)]
        public int? PageSize { get; set; }
    }
}
