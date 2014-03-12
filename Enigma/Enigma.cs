using Cryptography.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class Enigma
    {
        AlphabetUtils alphabet = new AlphabetUtils();

        public EnigmaDescriptor Descriptor { get; set; }

        public List<Rotor> Rotors { get { return rotors; } }

        private List<Rotor> rotors = new List<Rotor>();

        /// <summary>
        /// Creates an enigma from a list of rotor.
        /// </summary>
        /// <param name="rotors"></param>
        public Enigma(IEnumerable<Rotor> rotors)
        {
            if (rotors == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Rotor r in rotors)
            {
                if (r == null)
                {
                    throw new ArgumentNullException();
                }

                this.rotors.Add(r);
            }
        }

        public Enigma(IEnumerable<Rotor> rotors, EnigmaDescriptor descriptor)
        {
            if (rotors == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Rotor r in rotors)
            {
                if (r == null)
                {
                    throw new ArgumentNullException();
                }

                this.rotors.Add(r);
            }

            this.Descriptor = descriptor;
        }

        /// <summary>
        /// Press a key on the Enigma's keyboard and get the encrypted result.
        /// </summary>
        /// <param name="key">The letter typed on the Enigma's keyboard</param>
        /// <returns>The encrypted letter.</returns>
        /// <param name="permutations">Outputs a list of all permutations for this character.</param>
        public char PressKey(char key, out List<int> permutations)
        {
            char lastLetter = key;
            int lastIndex = AlphabetUtils.Instance[lastLetter];
            bool endOfCircuit = false;
            int currentRotorIndex = 0;
            Rotor currentRotor;
            int increment = 1;
            int lastOffset = 0;

            permutations = new List<int>(10);

            // The current flows through each drum and permutes the input.
            while (!endOfCircuit)
            {
                currentRotor = rotors[currentRotorIndex];

                lastOffset = currentRotor.GetOffsetFromKey(lastIndex);
                lastIndex += lastOffset * increment;

                if (lastIndex < 0)
                {
                    lastIndex = 26 + lastIndex;
                }
                else
                {
                    lastIndex = lastIndex % 26;
                }

                permutations.Add(lastOffset);

                // Make the current go back to the previous rotors.
                if (currentRotor.Type == Rotor.RotorType.Reflector)
                {
                    increment *= -1;
                }

                // Increment or decrement the current rotor.
                currentRotorIndex += increment;

                // End of circuit reached when the current hits the last rotor.
                if (currentRotorIndex == rotors.Count || currentRotorIndex < 0)
                {
                    endOfCircuit = true;
                }
            }

            return AlphabetUtils.Instance[lastIndex];
        }

        /// <summary>
        /// Press a key on the Enigma's keyboard and get the encrypted result.
        /// </summary>
        /// <param name="key">The letter typed on the Enigma's keyboard</param>
        /// <returns>The encrypted letter.</returns>
        public char PressKey(char key)
        {
            List<int> permutations;
            return PressKey(key, out permutations);
        }

        /// <summary>
        /// Used to format the output of the Enigma. Original does nothing.
        /// </summary>
        public enum OutputFormatting { Original, FiveLettersBlock, FourLettersBlock }

        /// <summary>
        /// Submits a text to the Enigma and get the encrypted result.
        /// </summary>
        /// <param name="input">The desired text.</param>
        /// <returns>The encrypted result</returns>
        public string SubmitString(string input, OutputFormatting formatting = OutputFormatting.Original)
        {
            if (input == null)
            {
                throw new ArgumentNullException("The input string cannot be null.");
            }

            if (input.Equals(string.Empty))
            {
                return string.Empty;
            }

            // Historical enigma only wrote upper case text.
            input = input.ToUpperInvariant();

            // And did not accept anything else than the 26 upper case letters of the roman alphabet.
            input = input.Replace(" ", "");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(PressKey(input[i]));
            }

            string result = sb.ToString();

            // The cryptographers also formatted the output in 4 or 5 letters blocks.
            result = FormatOutput(result, formatting);

            return result;
        }

        /// <summary>
        /// Format the Enigma's output according to a formatting scheme.
        /// The historical german formatting was 5 letters block for the army and air force, and 4
        /// letters block for the Navy.
        /// </summary>
        /// <param name="input">The string to format</param>
        /// <param name="formatting">The formatting scheme</param>
        /// <returns>The formatted string</returns>
        private static string FormatOutput(string input, OutputFormatting formatting)
        {
            int blockLength;

            switch (formatting)
            {
                case OutputFormatting.Original: blockLength = -1;
                    break;
                case OutputFormatting.FiveLettersBlock: blockLength = 6;
                    break;
                case OutputFormatting.FourLettersBlock: blockLength = 5;
                    break;
                default: blockLength = -1;
                    break;
            }

            if (blockLength > 0)
            {
                for (int i = 0; i < input.Length; i += blockLength)
                {
                    input = input.Insert(i, " ");
                }
            }

            return input;
        }

        /// <summary>
        /// The encryption key is made of as many letters as there are rotors in the device.
        /// It represents the initial rotation of each rotor. 
        /// The key "ABC" means that the first rotor must be set with the "A" letter in front of the notch.
        /// The second rotor must be set with the "B" letter in front of the notch, and the third rotor must be set 
        /// with the "C" letter in front of the notch.
        /// 
        /// Note that if one rotor is a stator (Always the first one in historical models), it will not move, hence it will not be part of the key.
        /// If there are 4 rotors and one stator, the key will be 4 letters long.  The reflector can move in some versions, so it counts as a rotor.
        /// </summary>
        /// <param name="key">The key must be only english alphabet letters</param>
        public void SetEncryptionKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("The encryption key must not be null, empty or contain whitespace.");
            }

            int rotatingRotors = 0;

            foreach (Rotor rotor in rotors)
            {
                if (rotor.Type != Rotor.RotorType.Stator)
                {
                    rotatingRotors++;
                }
            }

            if (key.Length != rotatingRotors)
            {
                throw new ArgumentException("The key must contain as many letters as rotors in the device.");
            }

            int k = 0;
            foreach (Rotor rotor in rotors)
            {
                if (rotor.Type != Rotor.RotorType.Stator)
                {
                    rotor.SetRotorKey(key[0]);
                    k++;
                }
            }
        }
    }
}
