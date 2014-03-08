using System;
using System.Text;

namespace Enigma.Elements
{
    /// <summary>
    /// A modelisation of the Enigma's rotor.
    /// Rotors were cylinders whose each side were made of 26 electrically connected dots, one for each letter.
    /// On one side, the current entered a dot, then exited through another dot on the opposite side.
    /// The mapping from dot to dot was the rotor's encryption key.
    /// </summary>
    public class Rotor
    {
        public enum RotorType { Alphabetical, ReverseAlphabetical, TypeI };

        private int length;
        private Connection[] rotor;
        private Connection[] initialRotor;
        private RotorType type;        

        /// <summary>
        /// Deflectors are special rotors that send the electric 
        /// current back to the previous rotor, transforming the x + 1 rotors in x*2 + 1.
        /// </summary>
        public bool IsDeflector { get; set; }

        public RotorType Type { get { return type; } }

        public int Length { get { return length; } }

        public Connection this[int index]
        {
            get { return rotor[index % length]; }
        }

        public char this[char key]
        {
            get { return GetValueFromKey(key); }
        }

        private char GetValueFromKey(char key)
        {
            foreach (Connection connection in rotor)
            {
                if (connection.Start.Equals(key))
                {
                    return connection.End;
                }
            }

            throw new ArgumentException(string.Format("The key {0} cannot be found in the rotor's connections."));
        }

        /// <summary>
        /// Offsets the rotor of x steps clockwise. 
        /// In reality, makes the drum rotate of x notches, setting new paths for the current.
        /// </summary>
        public void SetAtStep(int offset)
        {
            // Rebuilds the rotor
            Connection[] incrementedRotor = new Connection[length];

            for (int i = 0; i < length; i++)
            {
                incrementedRotor[i] = initialRotor[(i + offset) % length];
            }

            rotor = incrementedRotor;
        }

        /// <summary>
        /// Resets the rotor to its original position (The "A" position)
        /// </summary>
        public void Reset()
        {
            rotor = initialRotor;
        }

        /// <summary>
        /// Sets the rotor at a determined key. That is, a specific rotation of the rotor.
        /// Example : if the key is "B", then the rotor must be turned one notch clockwise.  If the key is "Z", then it must be turned 25 notches clockwise.
        /// </summary>
        /// <param name="key">Letter whose index sets the number of notches the rotor must be stepped.</param>
        public void SetRotorKey(char key)
        {
            int keyToInt = -1;

            for (int i = 0; i < AlphabetStruct.Alphabet.Length; i++)
            {
                if (AlphabetStruct.Alphabet[i].Equals(key))
                {
                    keyToInt = i;
                    break;
                }
            }

            if (keyToInt == -1)
            {
                throw new ArgumentOutOfRangeException(string.Format("The rotor key {0} is not a valid key", key));
            }

            SetAtStep(keyToInt);
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
            if (number < 1 || number > AlphabetStruct.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Connection[] result = new Connection[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = new Connection(AlphabetStruct.Alphabet[i], AlphabetStruct.Alphabet[i]);
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
            if (number < 1 || number > AlphabetStruct.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Connection[] result = new Connection[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = new Connection(AlphabetStruct.Alphabet[i], AlphabetStruct.Alphabet[number - 1 - i]);
            }

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rotor.Length; i++)
            {
                sb.Append(rotor[i].End);
            }

            return sb.ToString();
        }
    }
}
