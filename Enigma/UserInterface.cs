using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    class UserInterface
    {
        Factory factory;
        Enigma device;

        public UserInterface(Factory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("The factory cannot be null.");
            }

            this.factory = factory;
        }

        public void SetDevice(Enigma device)
        {
            this.device = device;
        }

        public Enigma RequestEnigmaTemplateChoice(Factory factory)
        {
            Console.WriteLine("CHOOSE MODEL :");

            int i = 1;
            foreach (var device in factory.EnigmaModels)
            {
                Console.Write(string.Format("{0} - {1}\n\n{2}", i, device.Value.Descriptor.Name, device.Value.Descriptor.Description));
                i++;
            }

            var choice = Console.Read();



            if (choice == (int)'1')
            {
                Console.WriteLine("\n You chose German Railway (Rocket)");
                return factory.EnigmaModels["GermanRailway"];
            }

            return null;
        }

        public void RequestEncryptionKey()
        {
            Console.Write("INPUT ENCRYPTION KEY > ");

            string key = Console.ReadLine();

            device.SetEncryptionKey(key);
            Console.WriteLine(string.Format("THE CURRENT ENCRYPTION KEY IS {0}", key));
        }

        public void RequestInputString()
        {
            Console.Write("INPUT TEXT TO ENCRYPT > ");

            string text = Console.ReadLine();
            
            Console.WriteLine(string.Format("THE ENCRYPTED TEXT IS : {0}", device.SubmitString(text, Enigma.OutputFormatting.FiveLettersBlock)));
        }
    }
}
