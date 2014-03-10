using Enigma.Elements;
using Enigma.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Enigma
{
    /// <summary>
    /// Class factory to create variants of the Enigma
    /// </summary>
    public class Factory
    {
        Dictionary<string, Rotor> rotorModels = new Dictionary<string, Rotor>();
        Dictionary<string, EnigmaDevice> enigmaModels = new Dictionary<string, EnigmaDevice>();

        public Factory(string uri)
        {
            DeserializeDataFile(uri);
        }

        private void DeserializeDataFile(string uri)
        {
            try
            {
                XDocument doc = XDocument.Load(uri);
                Deserialize(doc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Deserialize(XDocument doc)
        {
            GetRotorsTemplates(doc);
        }

        private IEnumerable<Rotor> GetRotorsTemplates(XDocument doc)
        {
            IEnumerable<XElement> rotorCollection;

            try
            {
                rotorCollection = doc.Root.Element("rotors").Elements("rotor");
            }

            catch (Exception)
            {
                throw;
            }

            var rotorDescriptors = from rotor in rotorCollection
                                   select new RotorDescriptor
                                   {
                                       Name = (string)rotor.Attribute("name"),
                                       Type = (string)rotor.Attribute("type"),
                                       Id = (string)rotor.Element("id"),
                                       Mapping = (string)rotor.Element("mapping"),
                                       Date = (string)rotor.Element("date"),
                                       Model = (string)rotor.Element("model")
                                   };

            List<Rotor> result = new List<Rotor>(rotorDescriptors.Count());

            foreach (var descriptor in rotorDescriptors)
            {
                Rotor rotor = new Rotor(descriptor);

                result.Add(rotor);
            }

            return result;
        }

        public EnigmaDevice CreateFromTemplate(string templateId)
        {
            if (enigmaModels.ContainsKey(templateId))
            {
                return enigmaModels[templateId];
            }

            throw new EnigmaTemplateNotFoundException(string.Format("No template was found for this template Id : {0}", templateId));
        }
    }
}
