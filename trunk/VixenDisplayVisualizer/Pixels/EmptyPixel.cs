// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.Windows.Media;

    /// <summary>
    ///   The empty pixel.
    /// </summary>
    public class EmptyPixel : IPixel
    {
        /// <summary>
        ///   Gets ChannelColor.
        /// </summary>
        public Color ChannelColor
        {
            get
            {
                return Colors.Black;
            }
        }

        /// <summary>
        ///   The contains.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <returns>
        ///   The contains.
        /// </returns>
        public bool Contains(Channel channel)
        {
            return false;
        }

        /// <summary>
        ///   The set color.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <param name = "intensity">
        ///   The intensity.
        /// </param>
        public void SetColor(Channel channel, byte intensity)
        {
            // Does nothing
        }
    }
}