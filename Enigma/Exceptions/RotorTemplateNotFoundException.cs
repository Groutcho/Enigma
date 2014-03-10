using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Exceptions
{
    public class RotorTemplateNotFoundException : Exception
    {
        public RotorTemplateNotFoundException()
            : base()
        {

        }

        public RotorTemplateNotFoundException(string message)
            : base(message)
        {

        }
    }
}
