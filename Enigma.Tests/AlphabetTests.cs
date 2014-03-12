using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cryptography.Elements;

namespace Cryptography.Tests
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void Shuffle()
        {
            var result = AlphabetUtils.Instance.GenerateRandomAlphabetOrder();
        }

        [TestMethod]
        public void IsValidMappingReturnsTrue()
        {
            Assert.IsTrue(AlphabetUtils.IsValidMapping(AlphabetUtils.AlphabetString));
            Assert.IsTrue(AlphabetUtils.IsValidMapping("FVPJIAOYEDRZXWGCTKUQSBNMHL"));
        }

        [TestMethod]
        public void IsValidMappingReturnsFalse()
        {
            // Repetition of letters
            Assert.IsFalse(AlphabetUtils.IsValidMapping("FVPCIAOYEDRZXWGCTKUQSBNMHL"));

            // 25 letters
            Assert.IsFalse(AlphabetUtils.IsValidMapping("FVPJIAOYERZXWGCTKUQSBNMHL"));

            // Empty / null input
            Assert.IsFalse(AlphabetUtils.IsValidMapping(""));
            Assert.IsFalse(AlphabetUtils.IsValidMapping(null));
        }
    }
}
