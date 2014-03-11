using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography.Elements
{
    /// <summary>
    /// Metadata of the enigma. Historical description, dates...
    /// </summary>
    public class EnigmaDescriptor
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Rotors { get; set; }
    }
}
