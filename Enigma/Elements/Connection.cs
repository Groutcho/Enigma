using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Elements
{
    /// <summary>
    /// In the enigma, the rotor were made of 26 electrical connections that mapped a letter X to another Y
    /// X -> Y . X is the key, Y is the  value.
    /// </summary>
    public struct Connection
    {
        private readonly char key;
        private readonly char value;

        public char Key { get { return key; } }
        public char Value { get { return value; } }

        public Connection(char key, char value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
