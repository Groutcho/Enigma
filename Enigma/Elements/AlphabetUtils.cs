using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Elements
{
    public class AlphabetUtils
    {
        private static AlphabetUtils instance;
        public static AlphabetUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AlphabetUtils();
                }
                return instance;
            }
        }

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

        /// <summary>
        /// Shuffle a set using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the set's elements</typeparam>
        /// <param name="input">The set to shuffle</param>
        /// <param name="random">The random number generator</param>
        /// <returns>The shuffled set</returns>
        public T[] Shuffle<T>(T[] input, Random random)
        {
            //To shuffle an array a of n elements (indices 0..n-1):
            //  for i from n − 1 downto 1 do
            //       j ← random integer with 0 ≤ j ≤ i
            //       exchange a[j] and a[i]

            T buffer;

            for (int i = input.Length -1; i > 0; i--)
            {
                int j = random.Next(0, i);
                buffer = input[j];
                input[j] = input[i];
                input[i] = buffer;
            }

            return input;
        }

        /// <summary>
        /// Generates a randomly shuffled alphabet using the Fisher-Yates shuffle.
        /// </summary>
        /// <returns>A randomly shuffled alphabet</returns>
        public string GenerateRandomAlphabetOrder()
        {
            var alphabet = (char[])Alphabet.Clone();

            Random random = new Random(DateTime.Now.Second);

            return new string(Shuffle<char>(alphabet, random));
        }

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
