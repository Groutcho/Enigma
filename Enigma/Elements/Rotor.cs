using System;
using System.Text;

namespace Enigma.Elements
{
    /// <summary>
    /// A modelisation of an Enigma rotor.
    /// Rotors were cylinders whose each side were made of 26 electrically connected dots, one for each letter.
    /// On one side, the current entered a dot, then exited through another dot on the opposite side.
    /// The mapping from dot to dot was the rotor's encryption key.
    /// 
    /// Deflectors were special rotors with electrical connections on only one side. 
    /// Their role was to send the electrical current back into the previous rotor.
    /// </summary>
    public class Rotor
    {
        public enum RotorType { Alphabetical, ReverseAlphabetical, TypeI };

        private int length;
        private Connection[] rotor;
        private Connection[] initialRotor;
        private RotorType type;
        private int offset;

        /// <summary>
        /// The rotation of the rotor. (In notches)
        /// If the rotor is set with the key "A", then the offset is 0.
        /// If the key is "Z" then the offset is 25.
        /// </summary>
        public int Offset { get { return offset; } }

        /// <summary>
        /// Deflectors are special rotors that send the electric 
        /// current back to the previous rotor, transforming the x + 1 rotors in x*2 + 1.
        /// </summary>
        public bool IsDeflector { get; set; }

        public RotorType Type { get { return type; } }

        public int Length { get { return length; } }

        /// <summary>
        /// Resets the rotor to its original position (The "A" position)
        /// </summary>
        public void Reset()
        {
            rotor = initialRotor;
            offset = 0;
        }

        /// <summary>
        /// Sets the rotor at a determined key. That is, a specific rotation of the rotor.
        /// Example : if the key is "B", then the rotor must be turned one notch clockwise.  If the key is "Z", then it must be turned 25 notches clockwise.
        /// </summary>
        /// <param name="key">Letter whose index sets the number of notches the rotor must be stepped.</param>
        public void SetRotorKey(char key)
        {
            int keyToInt = -1;

            for (int i = 0; i < AlphabetUtils.Alphabet.Length; i++)
            {
                if (AlphabetUtils.Alphabet[i].Equals(key))
                {
                    keyToInt = i;
                    break;
                }
            }

            if (keyToInt == -1)
            {
                throw new ArgumentOutOfRangeException(string.Format("The rotor key {0} is not a valid key", key));
            }

            OffsetRotor(keyToInt);
        }

        /// <summary>
        /// Offsets the rotor of x steps clockwise. That is, the rotor key is translated to an offset.
        /// In reality, makes the drum rotate of x notches, setting new paths for the current.
        /// </summary>
        public void OffsetRotor(int offset)
        {
            // Rebuilds the rotor
            Connection[] incrementedRotor = new Connection[length];

            for (int i = 0; i < length; i++)
            {
                incrementedRotor[i] = initialRotor[(i + offset) % length];
            }

            this.offset = offset;
            rotor = incrementedRotor;
        }

        public Rotor(Connection[] connections)
        {
            this.rotor = connections;
            this.initialRotor = connections;
            this.length = connections.Length;
        }

        public Rotor()
        {
            // Use the default rotor type.
            type = RotorType.ReverseAlphabetical;

            rotor = GenerateReverseAlphabeticalConnections(26);
            length = 26;
            this.initialRotor = rotor;
        }

        /// <summary>
        /// Creates a rotor of type type.
        /// </summary>
        /// <param name="type">The type of the rotor. For more info, refer to http://en.wikipedia.org/wiki/Enigma_rotor_details </param>
        public Rotor(RotorType type)
        {
            if (type == RotorType.Alphabetical)
            {
                rotor = GenerateAlphabeticalRotor(26);
                length = 26;
            }
            else if (type == RotorType.ReverseAlphabetical)
            {
                rotor = GenerateReverseAlphabeticalConnections(26);
                length = 26;
            }

            this.initialRotor = rotor;
        }

        /// <summary>
        /// Generates connections where each letter returns itself.
        /// </summary>
        /// <param name="number">the number of letter in the rotor</param>
        /// <returns>An array of connections.</returns>
        private Connection[] GenerateAlphabeticalRotor(int number)
        {
            if (number < 1 || number > AlphabetUtils.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Connection[] result = new Connection[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = new Connection(AlphabetUtils.Alphabet[i], AlphabetUtils.Alphabet[i]);
            }

            return result;
        }

        /// <summary>
        /// Generates an rotor of non random connections where the nth letter of the alphabet is mapped to the 26-nth letter.
        /// Example : A -> Z, B -> Y, ..., Z -> A
        /// </summary>
        /// <param name="number">The number of connections. Generally 26 for the whole alphabet.</param>
        /// <returns>An array of connections.</returns>
        private Connection[] GenerateReverseAlphabeticalConnections(int number)
        {
            if (number < 1 || number > AlphabetUtils.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Connection[] result = new Connection[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = new Connection(AlphabetUtils.Alphabet[i], AlphabetUtils.Alphabet[number - 1 - i]);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The entry point</param>
        /// <param name="reverse">If the electric current is flowing from the right to left.</param>
        /// <returns>The mapped exit point</returns>
        public int GetValueFromKey(int key, bool reverse)
        {
            //if (IsDeflector)
            //{
            //    reverse = false;
            //    key = key % length;
            //}
            //else
            //{
                key = (key + offset) % length;
            //}
            int result = -1;

            foreach (Connection connection in rotor)
            {
                if (reverse)
                {
                    if (connection.End.Equals(key))
                    {
                        result = key - connection.Offset;
                        break;
                    }
                }
                else
                {
                    if (connection.Start.Equals(key))
                    {
                        result = key + connection.Offset;
                        break;
                    }
                }
            }

            if (result >= 0)
            {
                return result % length;
            }
            else
            {
                throw new ArgumentException(string.Format("The key {0} cannot be found in the rotor's connections.", key));
            }
        }

        /// <summary>
        /// Returns an array of letters according to the mapping of the rotor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rotor.Length; i++)
            {
                sb.Append(AlphabetUtils.Instance[rotor[i].End]);
            }

            return sb.ToString();
        }
    }
}
