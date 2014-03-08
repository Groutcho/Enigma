using Enigma.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            Rotor rotor = new Rotor(Rotor.RotorType.Alphabetical);
            Rotor deflector = new Rotor(Rotor.RotorType.Alphabetical);
            deflector.IsDeflector = true;

            EnigmaDevice device = new EnigmaDevice(new Rotor[] { rotor, deflector });
            UserInterface userInterface = new UserInterface(device);

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
