using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class UserInterface
    {
        EnigmaDevice device;

        public UserInterface(EnigmaDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("The device cannot be null.");
            }

            this.device = device;
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
            
            Console.WriteLine(string.Format("THE ENCRYPTED TEXT IS : {0}", device.SubmitString(text, EnigmaDevice.OutputFormatting.FiveLettersBlock)));
        }
    }
}
