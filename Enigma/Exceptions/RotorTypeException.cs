using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cryptography.Elements
{
    public class RotorTypeException : Exception
    {
        public RotorTypeException(string message) : base (message)
        {
        }
    }
}
