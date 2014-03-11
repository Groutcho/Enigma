using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Cryptography.Exceptions;

namespace Cryptography.Tests
{
    [TestClass]
    public class Factory_Tests
    {
        private const string DATA_PATH = "F:/Developpement/C#/Studies/Enigma/Enigma/Data";

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ConstructorThrowsExceptionWhenWrongFolderUriIsPassed()
        {
            Factory factory = new Factory("DatO/enigma.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ConstructorThrowsExceptionWhenWrongFileUriIsPassed()
        {
            string dir = Path.Combine(DATA_PATH, "enigmU.xml");
            Factory factory = new Factory(dir);
        }

        [TestMethod]
        public void DataPathIsValid()
        {
            string dir = Path.Combine(DATA_PATH, "enigma.xml");

            Assert.IsTrue(File.Exists(dir));
        }

        [TestMethod]
        [ExpectedException(typeof(EnigmaTemplateNotFoundException))]
        public void TemplateNotFoundThrowsException()
        {
            string dir = Path.Combine(DATA_PATH, "enigma.xml");

            Factory factory = new Factory(dir);

            factory.CreateFromTemplate("invalidTemplateId");
        }

        [TestMethod]
        public void GenerateEnigmaTemplateSuccessfully()
        {
            string dir = Path.Combine(DATA_PATH, "enigma.xml");

            Factory factory = new Factory(dir);

            var GermanRailwayEnigma = factory.CreateFromTemplate("GermanRailway");

            Assert.AreEqual("GermanRailway", GermanRailwayEnigma.Descriptor.Id);
            Assert.AreEqual("German Railway (Rocket)", GermanRailwayEnigma.Descriptor.Name);
            Assert.AreEqual(5, GermanRailwayEnigma.Rotors.Count);
        }
    }
}
