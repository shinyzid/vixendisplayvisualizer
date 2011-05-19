// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    /// <summary>
    /// The red green blue pixel.
    /// </summary>
    public class RedGreenBluePixel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedGreenBluePixel"/> class.
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
        public RedGreenBluePixel(Channel red, Channel green, Channel blue)
        {
            this.RedChannel = red;
            this.GreenChannel = green;
            this.BlueChannel = blue;
        }

        /// <summary>
        /// Gets BlueChannel.
        /// </summary>
        public Channel BlueChannel { get; private set; }

        /// <summary>
        /// Gets GreenChannel.
        /// </summary>
        public Channel GreenChannel { get; private set; }

        /// <summary>
        /// Gets RedChannel.
        /// </summary>
        public Channel RedChannel { get; private set; }
    }
}