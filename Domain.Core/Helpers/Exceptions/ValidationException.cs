﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Helpers.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
