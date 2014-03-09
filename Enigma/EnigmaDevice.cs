using Enigma.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    public class EnigmaDevice
    {
        AlphabetUtils alphabet = new AlphabetUtils();

        public List<Rotor> Rotors { get { return rotors; } }

        private List<Rotor> rotors = new List<Rotor>();

        public EnigmaDevice(IEnumerable<Rotor> rotors)
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

        /// <summary>
        /// Press a key on the Enigma's keyboard and get the encrypted result.
        /// </summary>
        /// <param name="key">The letter typed on the Enigma's keyboard</param>
        /// <returns>The encrypted letter.</returns>
        public char PressKey(char key)
        {
            char lastLetter = key;
            int lastIndex = alphabet[lastLetter];
            bool endOfCircuit = false;
            bool reversed = false;
            int currentRotorIndex = 0;
            Rotor currentRotor;
            int offset = 1;

            while (!endOfCircuit)
            {
                currentRotor = rotors[currentRotorIndex];

                lastIndex = currentRotor.GetValueFromKey(lastIndex, reversed);

                // Make the current go back to the previous rotors.
                if (currentRotor.IsDeflector)
                {
                    offset *= -1;
                    reversed = !reversed;
                }

                // Increment or decrement the current rotor.
                currentRotorIndex += offset;

                // End of circuit reached.
                if (currentRotorIndex == rotors.Count || currentRotorIndex < 0)
                {
                    endOfCircuit = true;
                }
            }

            return alphabet[lastIndex];
        }

        public enum OutputFormatting { Concatenate, HistoricalWW2 }

        /// <summary>
        /// Submits a text to the Enigma and get the encrypted result.
        /// </summary>
        /// <param name="input">The desired text.</param>
        /// <returns>The encrypted result</returns>
        public string SubmitString(string input, OutputFormatting formatting = OutputFormatting.Concatenate)
        {
            if (input == null)
            {
                throw new ArgumentNullException("The input string cannot be null.");
            }

            if (input.Equals(string.Empty))
            {
                return string.Empty;
            }

            input = input.Replace(" ", "");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(PressKey(input[i]));
            }

            string result = sb.ToString();

            result = FormatOutput(result, formatting);          

            return result;
        }

        private static string FormatOutput(string result, OutputFormatting formatting)
        {
            if (formatting == OutputFormatting.HistoricalWW2)
            {
                for (int i = 0; i < result.Length; i += 6)
                {
                    result = result.Insert(i, " ");
                }
            }

            return result;
        }

        /// <summary>
        /// The encryption key is made of as many letters as there are rotors in the device.
        /// It represents the initial rotation of each rotor. 
        /// The key "ABC" means that the first rotor must be set with the "A" letter in front of the notch.
        /// The second rotor must be set with the "B" letter in front of the notch, and the third rotor must be set 
        /// with the "C" letter in front of the notch.
        /// </summary>
        /// <param name="key"></param>
        public void SetEncryptionKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("The encryption key must not be null, empty or contain whitespace.");
            }

            if (key.Length != rotors.Count)
            {
                throw new ArgumentException("The encryption key must contain as many letters as there are rotors in the device.");
            }

            for (int i = 0; i < key.Length; i++)
            {
                rotors[i].SetRotorKey(key[i]);
            }
        }
    }
}
