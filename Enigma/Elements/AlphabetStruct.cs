using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Elements
{
    public struct AlphabetStruct
    {
        public char this[int index]
        {
            get { return Alphabet[index]; }
        }

        public int this[char key]
        {
            get { return GetLetterIndex(key); }
        }

        public static readonly char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public const string AlphabetString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string ReverseAlphabetString = "ZYXWVUTSRQPONMLKJIHGFEDCBA";

        private int GetLetterIndex(char letter)
        {
            for (int i = 0; i < Alphabet.Length; i++)
            {
                if (letter.Equals(Alphabet[i]))
                {
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
