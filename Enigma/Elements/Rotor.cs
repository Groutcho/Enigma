using Cryptography.Exceptions;
using System;
using System.Text;

namespace Cryptography.Elements
{
    /// <summary>
    /// A modelisation of an Enigma rotor.
    /// Rotors were cylinders whose each side were made of 26 electrically connected dots, one for each letter.
    /// On one side, the current entered a dot, then exited through another dot on the opposite side.
    /// The wiring from dot to dot was the rotor's encryption key.
    /// 
    /// Example of historical wiring : UQNTLSZFMREHDPXKIBVYGJCWOA
    /// 
    /// Reflectors were special rotors with electrical connections on only one side.
    /// Their role was to send the electrical current back into the previous rotor and ensures the bijective property of the device.
    /// That is, X -> Y, and Y -> X
    /// </summary>
    public class Rotor
    {
        public enum RotorType { Stator, Rotor, Reflector }

        private int length;
        private Connection[] mapping;
        private Connection[] initialMapping;
        private RotorType type = RotorType.Rotor;
        private int offset;
        public RotorDescriptor Descriptor { get; set; }

        /// <summary>
        /// The rotation of the rotor. (In notches)
        /// If the rotor is set with the key "A", then the offset is 0.
        /// If the key is "Z" then the offset is 25.
        /// </summary>
        public int Offset { get { return offset; } }

        /// <summary>
        /// Reflectors are special rotors that send the electric 
        /// current back to the previous rotor, ensuring the bijective property of the rotor.
        /// </summary>
        public bool IsReflector { get; set; }

        public RotorType Type { get { return type; } }

        public int Length { get { return length; } }

        /// <summary>
        /// Resets the rotor to its original position (The "A" position)
        /// Same as setting the rotor key to "A"
        /// </summary>
        public void Reset()
        {
            mapping = initialMapping;
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
                incrementedRotor[i] = initialMapping[(i + offset) % length];
            }

            this.offset = offset;
            mapping = incrementedRotor;
        }

        public Rotor()
        {
            // Use the default rotor type.
            type = RotorType.Rotor;

            mapping = GenerateReverseAlphabeticalConnections(26);
            length = 26;
            this.initialMapping = mapping;
        }

        /// <summary>
        /// Generates a rotor from a mapping input.
        /// </summary>
        /// <param name="mapping">A rotor wiring such as "HQZGPJTMOBLNCIFDYAWVEUSRKX"</param>
        /// <param name="type">The rotor's type (rotor, stator, reflector)</param>
        public Rotor(string mapping, string type)
        {
            this.mapping = GenerateConnectionsFromMapping(mapping);
            this.initialMapping = this.mapping;

            this.length = this.mapping.Length;

            switch (type)
            {
                case "rotor": this.type = RotorType.Rotor;
                    break;

                case "stator": this.type = RotorType.Stator;
                    break;

                case "reflector": this.type = RotorType.Reflector;
                    break;

                default: throw new RotorTypeException(string.Format("The type {0} is not a valid rotor type", type));
            }
        }

        public Rotor(RotorDescriptor descriptor)
        {
            this.Descriptor = descriptor;

            this.mapping = GenerateConnectionsFromMapping(descriptor.Mapping);
            this.initialMapping = this.mapping;

            this.length = this.mapping.Length;

            switch (descriptor.Type)
            {
                case "rotor": this.type = RotorType.Rotor;
                    break;

                case "stator": this.type = RotorType.Stator;
                    break;

                case "reflector": this.type = RotorType.Reflector;
                    break;

                default: throw new RotorTypeException(string.Format("The type {0} is not a valid rotor type", type));
            }
        }        

        /// <summary>
        /// Generate an array of connection from the mapping input.
        /// </summary>
        /// <param name="mapping">A rotor wiring such as "HQZGPJTMOBLNCIFDYAWVEUSRKX"</param>
        /// <returns>An array of connections</returns>
        private Connection[] GenerateConnectionsFromMapping(string mapping)
        {
            bool valid = AlphabetUtils.IsValidMapping(mapping);

            if (valid)
            {
                Connection[] result = new Connection[mapping.Length];

                // Maps the regular alphabet order (ABCDEF...) to the input mapping.
                for (int i = 0; i < mapping.Length; i++)
                {
                    result[i] = new Connection(AlphabetUtils.Alphabet[i], mapping[i]);
                }

                return result;
            }

            else
            {
                throw new InvalidMappingException(string.Format("{0} is not a valid mapping", mapping));
            }
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
        /// Gets the mapped value for the input key.
        /// </summary>
        /// <param name="key">The entry point</param>
        /// <param name="reverse">If the electric current is flowing from the right to left.</param>
        /// <returns>The mapped exit point</returns>
        public int GetValueFromKey(int key, bool reverse)
        {
            key = (key + offset) % length;
            int result = -1;

            foreach (Connection connection in mapping)
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
        /// Returns the rotor's wiring as an array of letter.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mapping.Length; i++)
            {
                sb.Append(AlphabetUtils.Instance[mapping[i].End]);
            }

            return sb.ToString();
        }
    }
}
