using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cryptography.Elements;
using System.Collections.Generic;
using System.IO;

namespace Cryptography.Tests
{
    [TestClass]
    public class EnigmaDevice_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentConstructor()
        {
            Enigma enigma = new Enigma(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullListArgumentConstructor()
        {
            Rotor[] rotors = new Rotor[3];

            rotors[0] = null;
            rotors[1] = new Rotor();
            rotors[2] = new Rotor();

            Enigma enigma = new Enigma(rotors);
        }

        [TestMethod]
        public void ConstructorReturnsCorrectRotorCount()
        {
            Rotor[] rotors = new Rotor[4];

            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[1] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[2] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[3] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[3].IsReflector = true;

            Enigma enigma = new Enigma(rotors);
            Assert.AreEqual(4, enigma.Rotors.Count);
        }

        [TestMethod]
        public void PressKeyWithOneRotorReturnsCorrectLetter()
        {
            Rotor[] rotors = new Rotor[1];

            rotors[0] = new Rotor();

            Enigma enigma = new Enigma(rotors);

            Assert.AreEqual('Z', enigma.PressKey('A'));
            Assert.AreEqual('Y', enigma.PressKey('B'));
            Assert.AreEqual('X', enigma.PressKey('C'));
            Assert.AreEqual('W', enigma.PressKey('D'));
            Assert.AreEqual('V', enigma.PressKey('E'));
            Assert.AreEqual('U', enigma.PressKey('F'));
        }

        [TestMethod]
        public void SetEncryptionKeyReturnsCorrectTextForOneRotor()
        {
            const string TextToEncrypt = "HELLO";

            Rotor[] rotors = new Rotor[1];

            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");

            Enigma enigma = new Enigma(rotors);

            enigma.SetEncryptionKey("A");
            Assert.AreEqual(AlphabetUtils.AlphabetString, enigma.SubmitString(AlphabetUtils.AlphabetString));
            Assert.AreEqual(AlphabetUtils.ReverseAlphabetString, enigma.SubmitString(AlphabetUtils.ReverseAlphabetString));
            Assert.AreEqual(TextToEncrypt, enigma.SubmitString(TextToEncrypt));

            //           E    H     L    O
            // A -> ABCD E FG H IJK L MN O PQRSTUVWXYZ
            // B -> BCDE F GH I JKL M NO P QRSTUVWXYZA
            // H -> HIJK L MN O PQR S TU V WXYZABCDEFG
            // Z -> ZABC D EF G HIJ K LM N OPQRSTUVWXY

            enigma.SetEncryptionKey("B");
            Assert.AreEqual("IFMMP", enigma.SubmitString(TextToEncrypt));

            enigma.SetEncryptionKey("H");
            Assert.AreEqual("OLSSV", enigma.SubmitString(TextToEncrypt));

            enigma.SetEncryptionKey("Z");
            Assert.AreEqual("GDKKN", enigma.SubmitString(TextToEncrypt));
        }

        [TestMethod]
        public void SetEncryptionKeyReturnsCorrectTextForOneRotorAndOneDeflectorAndOneLetterInput()
        {
            const string plaintext = "A";

            Rotor[] rotors = new Rotor[2];

            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[1] = new Rotor(AlphabetUtils.ReverseAlphabetString, "reflector");

            Enigma enigma = new Enigma(rotors);

            enigma.SetEncryptionKey("BB");

            string ciphertext = enigma.SubmitString(plaintext);

            Assert.AreEqual(plaintext, enigma.SubmitString(ciphertext), "Encryption symmetry is not ensured.");
        }

        [TestMethod]
        public void SymmetryIsEnsuredWithOneFixedRotorAndReflector()
        {
            const string plaintext = "HELLO";

            Rotor[] rotors = new Rotor[2];
            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[1] = new Rotor(AlphabetUtils.ReverseAlphabetString, "reflector");

            Enigma enigma = new Enigma(rotors);

            enigma.SetEncryptionKey("BB");

            string ciphertext = enigma.SubmitString(plaintext);

            Assert.AreEqual(plaintext, enigma.SubmitString(ciphertext), "Encryption symmetry is not ensured.");
        }

        private const string DATA_PATH = "F:/Developpement/C#/Studies/Enigma/Enigma/Data";

        [TestMethod]
        public void SymmetryIsEnsuredWithAnyPresetAndZeroKey()
        {
            string dir = Path.Combine(DATA_PATH, "enigma.xml");
            Factory factory = new Factory(dir);
            Enigma enigma = factory.CreateFromTemplate("GermanRailway");

            enigma.SetEncryptionKey("AAAAA");

            const string plaintext = "HELLOMYNAMEISJOHN";

            string ciphertext = enigma.SubmitString(plaintext, Enigma.OutputFormatting.Original);

            Assert.AreEqual(plaintext, enigma.SubmitString(ciphertext, Enigma.OutputFormatting.Original), "Encryption symmetry is not ensured.");
        }

        [TestMethod]
        public void SymmetryIsEnsuredWithAnyPresetAndAnyKey()
        {
            string dir = Path.Combine(DATA_PATH, "enigma.xml");
            Factory factory = new Factory(dir);
            Enigma enigma = factory.CreateFromTemplate("GermanRailway");

            enigma.SetEncryptionKey("ZQRXO");

            const string plaintext = "HELLOMYNAMEISJOHN";

            string ciphertext = enigma.SubmitString(plaintext, Enigma.OutputFormatting.Original);

            Assert.AreEqual(plaintext, enigma.SubmitString(ciphertext, Enigma.OutputFormatting.Original), "Encryption symmetry is not ensured.");
        }
    }
}
