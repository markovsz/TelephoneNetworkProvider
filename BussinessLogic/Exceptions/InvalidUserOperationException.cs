﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Exceptions
{
    public class InvalidUserOperationException : Exception
    {
        public InvalidUserOperationException(string message)
            : base(message)
        {
        }
    }
}