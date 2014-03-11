using Cryptography.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            Rotor rotor = new Rotor(AlphabetUtils.ReverseAlphabetString, "rotor");
            Rotor reflector = new Rotor(AlphabetUtils.ReverseAlphabetString, "reflector");

            Enigma device = new Enigma(new Rotor[] { rotor, reflector });

            const string DATA_PATH = "F:/Developpement/C#/Studies/Enigma/Enigma/Data";
            string dir = Path.Combine(DATA_PATH, "enigma.xml");

            Factory factory = new Factory(dir);

            UserInterface userInterface = new UserInterface(factory);

            Enigma choice = userInterface.RequestEnigmaTemplateChoice(factory);

            userInterface.SetDevice(choice);

            while (true)
            {
                try
                {
                    userInterface.RequestEncryptionKey();
                    userInterface.RequestInputString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
