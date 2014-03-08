using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Enigma.Elements;

namespace Enigma.Tests
{
    [TestClass]
    public class RotorTests
    {
        [TestMethod]
        public void RotorConstructor()
        {
            Rotor rotor = new Rotor();

            Assert.AreEqual(26, rotor.Length);
        }

        [TestMethod]
        public void AlphabeticalRotorReturnsCorrectMapping()
        {
            Rotor rotor = new Rotor(Rotor.RotorType.Alphabetical);

            foreach (char letter in Rotor.Alphabet)
            {
                Assert.AreEqual(letter, rotor[letter]);
            }
        }

        [TestMethod]
        public void AlphabeticalRotorReturnsCorrectMappingForIndex()
        {
            Rotor rotor = new Rotor(Rotor.RotorType.Alphabetical);

            for (int i = 0; i < rotor.Length; i++)
            {
                Assert.AreEqual(Rotor.Alphabet[i], rotor[i].Key);
            }
        }

        [TestMethod]
        public void AlphabeticalRotorStepsCorrectly()
        {
            Rotor rotor = new Rotor(Rotor.RotorType.Alphabetical);

            // Step one notch.
            rotor.SetRotorKey('B');

            Assert.AreEqual('B', rotor[0].Key);
            Assert.AreEqual('C', rotor[1].Key);
            Assert.AreEqual('D', rotor[2].Key);
            Assert.AreEqual('E', rotor[3].Key);
            Assert.AreEqual('F', rotor[4].Key);
            Assert.AreEqual('G', rotor[5].Key);
            Assert.AreEqual('H', rotor[6].Key);
            Assert.AreEqual('I', rotor[7].Key);
            Assert.AreEqual('J', rotor[8].Key);
            Assert.AreEqual('K', rotor[9].Key);
            Assert.AreEqual('L', rotor[10].Key);

            Assert.AreEqual('M', rotor[11].Key);
            Assert.AreEqual('N', rotor[12].Key);
            Assert.AreEqual('O', rotor[13].Key);
            Assert.AreEqual('P', rotor[14].Key);
            Assert.AreEqual('Q', rotor[15].Key);
            Assert.AreEqual('R', rotor[16].Key);
            Assert.AreEqual('S', rotor[17].Key);
            Assert.AreEqual('T', rotor[18].Key);
            Assert.AreEqual('U', rotor[19].Key);
            Assert.AreEqual('V', rotor[20].Key);

            Assert.AreEqual('W', rotor[21].Key);
            Assert.AreEqual('X', rotor[22].Key);
            Assert.AreEqual('Y', rotor[23].Key);
            Assert.AreEqual('Z', rotor[24].Key);
            Assert.AreEqual('A', rotor[25].Key);
        }
    }
}
