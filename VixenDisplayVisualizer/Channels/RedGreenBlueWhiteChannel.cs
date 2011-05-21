// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    /// <summary>
    /// The red green blue white channel.
    /// </summary>
    internal class RedGreenBlueWhiteChannel : RedGreenBlueChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedGreenBlueWhiteChannel"/> class.
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
        /// <param name="white">
        /// The white.
        /// </param>
        public RedGreenBlueWhiteChannel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            this.WhiteChannel = white;
        }

        /// <summary>
        /// Gets ChannelColor.
        /// </summary>
        public override Color ChannelColor
        {
            get
            {
                return Color.Transparent;
            }
        }

        /// <summary>
        ///   Gets WhiteChannel.
        /// </summary>
        public Channel WhiteChannel { get; private set; }
    }
}