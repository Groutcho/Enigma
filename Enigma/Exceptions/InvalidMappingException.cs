using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enigma.Exceptions
{
    public class InvalidMappingException : Exception
    {
        public InvalidMappingException(string message) : base(message)
        {

        }
    }
}
