using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography.Elements
{
    /// <summary>
    /// In the Enigma, the rotors were made of 26 electrical connections that mapped a letter X to another Y.
    /// </summary>
    public struct Connection
    {
        private readonly int start;
        private readonly int end;
        private readonly int offset;

        public int Start { get { return start; } }
        public int End { get { return end; } }

        /// <summary>
        /// The function that maps X -> Y
        /// </summary>
        public int Offset { get { return offset; } }

        public Connection(char start, char end)
        {
            var alphabet = new AlphabetUtils();

            // Maps the letter to its corresponding number (A = 0, Z = 25)
            this.start = alphabet[start];
            this.end = alphabet[end];
            this.offset = end - start;
        }

        public override string ToString()
        {
            return string.Format("{0} <-> {1}", start, end);
        }
    }
}
