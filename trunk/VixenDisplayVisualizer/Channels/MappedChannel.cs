// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    /// <summary>
    /// The mapped channel.
    /// </summary>
    public class MappedChannel : IChannel
    {
        /// <summary>
        /// The _channel.
        /// </summary>
        private readonly IChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedChannel"/> class.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        public MappedChannel(IChannel channel)
        {
            this._channel = channel;
        }

        /// <summary>
        /// Gets ChannelColor.
        /// </summary>
        public Color ChannelColor
        {
            get
            {
                return this._channel.ChannelColor;
            }
        }

        public string Name { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}