// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    /// <summary>
    /// The single channel pixel.
    /// </summary>
    public class SingleChannelPixel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleChannelPixel"/> class.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        public SingleChannelPixel(Channel channel)
        {
            this.Channel = channel;
        }

        /// <summary>
        /// Gets Channel.
        /// </summary>
        public Channel Channel { get; private set; }
    }
}