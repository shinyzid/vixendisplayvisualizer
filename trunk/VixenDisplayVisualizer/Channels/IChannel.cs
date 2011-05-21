// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    /// <summary>
    /// The channel interface.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Gets ChannelColor.
        /// </summary>
        Color ChannelColor { get; }
    }
}