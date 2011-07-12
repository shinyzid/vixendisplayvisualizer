// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------

namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Xml;

    public static class Extensions
    {
        public static int TryParseInt32(this string stringValue, int fallbackValue)
        {
            int result;
            if (!int.TryParse(stringValue, out result))
            {
                result = fallbackValue;
            }

            return result;
        }

        public static void AppendAttribute(this XmlNode node, string name, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }
    }
}