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

            foreach (char letter in AlphabetStruct.Alphabet)
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
                Assert.AreEqual(AlphabetStruct.Alphabet[i], rotor[i].Start);
            }
        }

        [TestMethod]
        public void AlphabeticalRotorStepsCorrectly()
        {
            Rotor rotor = new Rotor(Rotor.RotorType.Alphabetical);

            // Step one notch.
            rotor.SetRotorKey('B');

            Assert.AreEqual('B', rotor[0].Start);
            Assert.AreEqual('C', rotor[1].Start);
            Assert.AreEqual('D', rotor[2].Start);
            Assert.AreEqual('E', rotor[3].Start);
            Assert.AreEqual('F', rotor[4].Start);
            Assert.AreEqual('G', rotor[5].Start);
            Assert.AreEqual('H', rotor[6].Start);
            Assert.AreEqual('I', rotor[7].Start);
            Assert.AreEqual('J', rotor[8].Start);
            Assert.AreEqual('K', rotor[9].Start);
            Assert.AreEqual('L', rotor[10].Start);

            Assert.AreEqual('M', rotor[11].Start);
            Assert.AreEqual('N', rotor[12].Start);
            Assert.AreEqual('O', rotor[13].Start);
            Assert.AreEqual('P', rotor[14].Start);
            Assert.AreEqual('Q', rotor[15].Start);
            Assert.AreEqual('R', rotor[16].Start);
            Assert.AreEqual('S', rotor[17].Start);
            Assert.AreEqual('T', rotor[18].Start);
            Assert.AreEqual('U', rotor[19].Start);
            Assert.AreEqual('V', rotor[20].Start);

            Assert.AreEqual('W', rotor[21].Start);
            Assert.AreEqual('X', rotor[22].Start);
            Assert.AreEqual('Y', rotor[23].Start);
            Assert.AreEqual('Z', rotor[24].Start);
            Assert.AreEqual('A', rotor[25].Start);
        }
    }
}
