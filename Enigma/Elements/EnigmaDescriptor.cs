using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Data
{
    /// <summary>
    /// Metadata of the enigma. Historical description, dates...
    /// </summary>
    public class EnigmaDescriptor
    {
        public string Name { get; }
        public string Description { get; }
        public string DateOfIntroduction { get; }
    }
}
