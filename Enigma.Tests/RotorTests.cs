using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Enigma.Elements;

namespace Enigma.Tests
{
    public static class Globals
    {
        public static readonly char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    }


    [TestClass]
    public class RotorTests
    {
        private Connection[] GenerateFixedConnections(int number)
        {
            if (number < 1 || number > Globals.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            Connection[] result = new Connection[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = new Connection(Globals.Alphabet[i], Globals.Alphabet[number-i]);
            }

            return result;
        }

        [TestMethod]
        public void RotorConstructor()
        {
            Connection[] connections = GenerateFixedConnections(26);

            Rotor rotor = new Rotor(connections);

            Assert.AreEqual(26, rotor.Length);
        }

        [TestMethod]
        public void RotorConstructor()
        {
            Connection[] connections = GenerateFixedConnections(26);

            Rotor rotor = new Rotor(connections);

            Assert.AreEqual(26, rotor.Length);
        }      
    }
}
