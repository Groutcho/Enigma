using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Exceptions
{
    public class EnigmaTemplateNotFoundException : Exception
    {
        public EnigmaTemplateNotFoundException()
            : base()
        {

        }

        public EnigmaTemplateNotFoundException(string message)
            : base(message)
        {

        }
    }
}
