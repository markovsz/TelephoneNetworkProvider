﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RequestParameters
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
