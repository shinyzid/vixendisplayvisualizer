// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    /// <summary>
    /// The red green blue pixel.
    /// </summary>
    public class RedGreenBlueChannel : IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedGreenBlueChannel"/> class.
        /// </summary>
        /// <param name="red">
        /// The red.
        /// </param>
        /// <param name="green">
        /// The green.
        /// </param>
        /// <param name="blue">
        /// The blue.
        /// </param>
        public RedGreenBlueChannel(Channel red, Channel green, Channel blue)
        {
            this.RedChannel = red;
            this.GreenChannel = green;
            this.BlueChannel = blue;
        }

        /// <summary>
        ///   Gets BlueChannel.
        /// </summary>
        public Channel BlueChannel { get; private set; }

        /// <summary>
        /// Gets ChannelColor.
        /// </summary>
        public virtual Color ChannelColor
        {
            get
            {
                return Color.Transparent;
            }
        }

        /// <summary>
        ///   Gets GreenChannel.
        /// </summary>
        public Channel GreenChannel { get; private set; }

        /// <summary>
        ///   Gets RedChannel.
        /// </summary>
        public Channel RedChannel { get; private set; }
    }
}