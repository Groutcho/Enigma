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

        public Dictionary<string, EnigmaDevice> EnigmaModels { get { return enigmaModels; } }

        public Factory(string uri)
        {
            DeserializeDataFile(uri);
        }

        /// <summary>
        /// Deserializes an xml file containing all models of rotors and Enigma presets.
        /// </summary>
        /// <param name="uri">A valid URI</param>
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
            foreach (var template in GetRotorsTemplates(doc))
            {
                rotorModels.Add(template.Descriptor.Id, template);
            }

            foreach (var template in GetEnigmaPresets(doc))
            {
                enigmaModels.Add(template.Descriptor.Id, template);
            }            
        }

        private IEnumerable<EnigmaDevice> GetEnigmaPresets(XDocument doc)
        {
            IEnumerable<XElement> enigmaCollection;

            try
            {
                enigmaCollection = doc.Root.Element("presets").Elements("device");
            }

            catch (Exception)
            {
                throw;
            }

            var enigmaDescriptors = from enigma in enigmaCollection
                                   select new EnigmaDescriptor
                                   {
                                       Id = (string)enigma.Element("id"),
                                       Name = (string)enigma.Attribute("name"),
                                       Description = (string)enigma.Element("description"),
                                       Rotors = from  rotor in enigma.Element("rotors").Elements("rotor") select rotor.Value
                                   };

            List<EnigmaDevice> result = new List<EnigmaDevice>(enigmaDescriptors.Count());

            foreach (var descriptor in enigmaDescriptors)
            {
                var rotors = from rotor in descriptor.Rotors select rotorModels[rotor];

                EnigmaDevice device = new EnigmaDevice(rotors, descriptor);

                result.Add(device);
            }

            return result;
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
