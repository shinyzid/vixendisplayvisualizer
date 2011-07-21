namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Xml;

    /// <summary>
    ///   The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///   The append attribute.
        /// </summary>
        /// <param name = "node">
        ///   The node.
        /// </param>
        /// <param name = "name">
        ///   The name.
        /// </param>
        /// <param name = "value">
        ///   The value.
        /// </param>
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

        /// <summary>
        ///   The get attribute value.
        /// </summary>
        /// <param name = "node">
        ///   The node.
        /// </param>
        /// <param name = "attributeName">
        ///   The attribute name.
        /// </param>
        /// <returns>
        ///   The get attribute value.
        /// </returns>
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

        /// <summary>
        ///   The notify property changed.
        /// </summary>
        /// <param name = "propertyChangedEventHandler">
        ///   The property changed event handler.
        /// </param>
        /// <param name = "propertyName">
        ///   The property name.
        /// </param>
        /// <param name = "sender">
        ///   The sender.
        /// </param>
        public static void NotifyPropertyChanged(
            this PropertyChangedEventHandler propertyChangedEventHandler, string propertyName, object sender)
        {
            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        ///   The try parse int 32.
        /// </summary>
        /// <param name = "stringValue">
        ///   The string value.
        /// </param>
        /// <param name = "fallbackValue">
        ///   The fallback value.
        /// </param>
        /// <returns>
        ///   The try parse int 32.
        /// </returns>
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
