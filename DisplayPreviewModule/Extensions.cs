namespace Vixen.Modules.DisplayPreviewModule
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Xml;

    public static class Extensions
    {
        public static void AppendAttribute(this XmlNode node, string name, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }

        public static byte[] ToByteArray(this BitmapSource imageSource)
        {
            if (imageSource == null)
            {
                return new byte[0];
            }

            var memStream = new MemoryStream();
            var encoder = new JpegBitmapEncoder { QualityLevel = 30 };
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(memStream);
            return memStream.GetBuffer();
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
    }
}
