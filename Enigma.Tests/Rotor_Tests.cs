using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cryptography.Elements;

namespace Cryptography.Tests
{
    [TestClass]
    public class Rotor_Tests
    {
        AlphabetUtils alphabet = new AlphabetUtils();

        [TestMethod]
        public void RotorConstructor()
        {
            Rotor rotor = new Rotor();

            Assert.AreEqual(26, rotor.Length);
        }

        [TestMethod]
        public void AlphabeticalRotorReturnsCorrectMapping()
        {
            Rotor rotor = new Rotor(AlphabetUtils.AlphabetString, "rotor");

            foreach (char letter in AlphabetUtils.Alphabet)
            {
                Assert.AreEqual(letter, alphabet[rotor.GetValueFromKey(alphabet[letter], false)]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RotorTypeException))]
        public void WrontRotorTypeGeneratesException()
        {
            Rotor rotor = new Rotor(AlphabetUtils.AlphabetString, "error");
        }

        [TestMethod]
        public void AlphabeticalRotorStepsCorrectly()
        {
            Rotor rotor = new Rotor(AlphabetUtils.AlphabetString, "rotor");

            // Step one notch.
            rotor.SetRotorKey('B');

            Assert.AreEqual('B', alphabet[rotor.GetValueFromKey(alphabet['A'], false)]);
            Assert.AreEqual('C', alphabet[rotor.GetValueFromKey(alphabet['B'], false)]);
            Assert.AreEqual('D', alphabet[rotor.GetValueFromKey(alphabet['C'], false)]);
            Assert.AreEqual('E', alphabet[rotor.GetValueFromKey(alphabet['D'], false)]);
            Assert.AreEqual('F', alphabet[rotor.GetValueFromKey(alphabet['E'], false)]);
            Assert.AreEqual('G', alphabet[rotor.GetValueFromKey(alphabet['F'], false)]);
            Assert.AreEqual('H', alphabet[rotor.GetValueFromKey(alphabet['G'], false)]);
            Assert.AreEqual('I', alphabet[rotor.GetValueFromKey(alphabet['H'], false)]);
            Assert.AreEqual('J', alphabet[rotor.GetValueFromKey(alphabet['I'], false)]);
            Assert.AreEqual('K', alphabet[rotor.GetValueFromKey(alphabet['J'], false)]);
            Assert.AreEqual('L', alphabet[rotor.GetValueFromKey(alphabet['K'], false)]);
            Assert.AreEqual('M', alphabet[rotor.GetValueFromKey(alphabet['L'], false)]);
            Assert.AreEqual('N', alphabet[rotor.GetValueFromKey(alphabet['M'], false)]);
            Assert.AreEqual('O', alphabet[rotor.GetValueFromKey(alphabet['N'], false)]);
            Assert.AreEqual('P', alphabet[rotor.GetValueFromKey(alphabet['O'], false)]);
            Assert.AreEqual('Q', alphabet[rotor.GetValueFromKey(alphabet['P'], false)]);
            Assert.AreEqual('R', alphabet[rotor.GetValueFromKey(alphabet['Q'], false)]);
            Assert.AreEqual('S', alphabet[rotor.GetValueFromKey(alphabet['R'], false)]);
            Assert.AreEqual('T', alphabet[rotor.GetValueFromKey(alphabet['S'], false)]);
            Assert.AreEqual('U', alphabet[rotor.GetValueFromKey(alphabet['T'], false)]);
            Assert.AreEqual('V', alphabet[rotor.GetValueFromKey(alphabet['U'], false)]);
            Assert.AreEqual('W', alphabet[rotor.GetValueFromKey(alphabet['V'], false)]);
            Assert.AreEqual('X', alphabet[rotor.GetValueFromKey(alphabet['W'], false)]);
            Assert.AreEqual('Y', alphabet[rotor.GetValueFromKey(alphabet['X'], false)]);
            Assert.AreEqual('Z', alphabet[rotor.GetValueFromKey(alphabet['Y'], false)]);
            Assert.AreEqual('A', alphabet[rotor.GetValueFromKey(alphabet['Z'], false)]);
        }
    }
}
