using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace Cuit.Helpers
{
    public class XmlParsingException : Exception
    {
        public XmlParsingException(string message) : base(message) { }
    }

    public static class FormXmlParser
    {
        public static Dictionary<string, IControl> Parse(string xmlPath)
        {
            var retval = new Dictionary<string, IControl>();

            var document = XDocument.Load(xmlPath);
            var ns = document.Root.GetDefaultNamespace();

            var controlsContainer = document.Descendants(ns + "Controls").FirstOrDefault();
            if (controlsContainer == null)
            {
                throw new XmlParsingException("Xml doesn't contain a <Control> element");
            }

            foreach (var controlElement in controlsContainer.Elements())
            {
                var typename = $"Cuit.Control.{controlElement.Name.LocalName}";
                var type = Type.GetType(typename, false);
                if (type == null)
                {
                    throw new XmlParsingException($"Unable to find a type for control with name {controlElement.Name.LocalName}");
                }

                var leftAttribute = controlElement.Attribute("Left");
                var topAttribute = controlElement.Attribute("Top");
                var name = controlElement.Attribute("Name");

                if (leftAttribute == null || topAttribute == null)
                {
                    throw new XmlParsingException($"Left and Top attributes are mandatory for control with name {controlElement.Name.LocalName}");
                }

                var controlObject = (IControl)Activator.CreateInstance(type, new object[] { Convert.ToInt32(leftAttribute.Value), Convert.ToInt32(topAttribute.Value) });
                var controlProperties = type.GetRuntimeProperties();

                foreach (var otherAttribute in controlElement.Attributes().Where(a => a.Name.LocalName != "Left" && a.Name.LocalName != "Top" && a.Name.LocalName != "Name"))
                {
                    var property = controlProperties.FirstOrDefault(p => p.Name == otherAttribute.Name.LocalName);
                    if (property != null)
                    {
                        object value;

                        try
                        {
                            if (property.PropertyType.GetTypeInfo().IsEnum)
                            {
                                value = Enum.Parse(property.PropertyType, otherAttribute.Value);
                            }
                            else
                            {
                                value = Convert.ChangeType(otherAttribute.Value, property.PropertyType);
                            }
                        }
                        catch
                        {
                            throw new XmlParsingException($"Unable to convert value '{otherAttribute.Value}' to type '{property.PropertyType}'");
                        }


                        property.SetValue(controlObject, value);
                    }
                }

                retval.Add(name?.Value ?? Guid.NewGuid().ToString(), controlObject);
            }

            return retval;
        }
    }
}
