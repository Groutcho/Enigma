﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Enigma.Elements;
using System.Collections.Generic;

namespace Enigma.Tests
{
    [TestClass]
    public class EnigmaDevice_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentConstructor()
        {
            EnigmaDevice enigma = new EnigmaDevice(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullListArgumentConstructor()
        {
            Rotor[] rotors = new Rotor[3];

            rotors[0] = null;
            rotors[1] = new Rotor();
            rotors[2] = new Rotor();

            EnigmaDevice enigma = new EnigmaDevice(rotors);
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

            EnigmaDevice enigma = new EnigmaDevice(rotors);
            Assert.AreEqual(4, enigma.Rotors.Count);
        }

        [TestMethod]
        public void PressKeyWithOneRotorReturnsCorrectLetter()
        {
            Rotor[] rotors = new Rotor[1];

            rotors[0] = new Rotor();
            
            EnigmaDevice enigma = new EnigmaDevice(rotors);

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

            EnigmaDevice enigma = new EnigmaDevice(rotors);

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
            const string TextToEncrypt = "A";

            Rotor[] rotors = new Rotor[2];

            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[1] = new Rotor(AlphabetUtils.ReverseAlphabetString, "reflector");

            EnigmaDevice enigma = new EnigmaDevice(rotors);

            enigma.SetEncryptionKey("BB");

            string encryptedText = enigma.SubmitString(TextToEncrypt);

            Assert.AreEqual(TextToEncrypt, enigma.SubmitString(encryptedText));
        }

        [TestMethod]
        public void SetEncryptionKeyReturnsCorrectTextForOneRotorAndOneDeflector()
        {
            const string TextToEncrypt = "HELLO";

            Rotor[] rotors = new Rotor[2];
            rotors[0] = new Rotor(AlphabetUtils.AlphabetString, "rotor");
            rotors[1] = new Rotor(AlphabetUtils.ReverseAlphabetString, "reflector");

            EnigmaDevice enigma = new EnigmaDevice(rotors);

            enigma.SetEncryptionKey("BB");

            string encryptedText = enigma.SubmitString(TextToEncrypt);

            Assert.AreEqual(TextToEncrypt, enigma.SubmitString(encryptedText));
        }
    }
}
