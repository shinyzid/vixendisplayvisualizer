// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.ComponentModel;
    using System.Xml;

    public static class Extensions
    {
        public static void AppendAttribute(this XmlNode node, string name, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }

        public static void NotifyPropertyChanged(
            this PropertyChangedEventHandler propertyChangedEventHandler, string propertyName, object sender)
        {
            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static int TryParseInt32(this string stringValue, int fallbackValue)
        {
            int result;
            if (!int.TryParse(stringValue, out result))
            {
                result = fallbackValue;
            }

            return result;
        }

        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            if (node == null)
            {
                return null;
            }

            var attribute = node.Attributes.GetNamedItem(attributeName);
            if (attribute == null)
            {
                return null;
            }

            return attribute.Value;
        }
    }
}