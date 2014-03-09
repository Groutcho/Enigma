using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Enigma.Elements;

namespace Enigma.Tests
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void Shuffle()
        {
            var result = AlphabetUtils.Instance.GenerateRandomAlphabetOrder();
        }
    }
}
