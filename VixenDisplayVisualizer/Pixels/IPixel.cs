// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.Windows.Media;

    /// <summary>
    ///   The i pixel.
    /// </summary>
    public interface IPixel
    {
        /// <summary>
        ///   Gets ChannelColor.
        /// </summary>
        Color ChannelColor { get; }

        /// <summary>
        ///   The contains.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <returns>
        ///   The contains.
        /// </returns>
        bool Contains(Channel channel);

        /// <summary>
        ///   The set color.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <param name = "intensity">
        ///   The intensity.
        /// </param>
        void SetColor(Channel channel, byte intensity);
    }
}