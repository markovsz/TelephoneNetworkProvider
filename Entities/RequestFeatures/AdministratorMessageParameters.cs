﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class AdministratorMessageParameters : RequestParameters
    {
        public string UserId { get; set; }
        public string Status { get; set; }
    }
}
