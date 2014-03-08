using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Elements
{
    /// <summary>
    /// In the Enigma, the rotors were made of 26 electrical connections that mapped a letter X to another Y.
    /// </summary>
    public struct Connection
    {
        private readonly char start;
        private readonly char end;

        public char Start { get { return start; } }
        public char End { get { return end; } }

        public Connection(char start, char end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
