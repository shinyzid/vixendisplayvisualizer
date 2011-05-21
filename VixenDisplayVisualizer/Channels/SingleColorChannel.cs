// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    /// <summary>
    /// The single channel pixel.
    /// </summary>
    public class SingleColorChannel : IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleColorChannel"/> class.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        public SingleColorChannel(Channel channel)
        {
            this.Channel = channel;
        }

        /// <summary>
        ///   Gets Channel.
        /// </summary>
        public Channel Channel { get; private set; }

        /// <summary>
        /// Gets ChannelColor.
        /// </summary>
        public Color ChannelColor
        {
            get
            {
                return Color.Transparent;
            }
        }
    }
}