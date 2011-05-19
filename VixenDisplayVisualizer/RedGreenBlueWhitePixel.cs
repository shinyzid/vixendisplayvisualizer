// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedGreenBlueWhitePixel.cs" company="Erik Mathisen">
//   2011
// </copyright>
// <summary>
//   The red green blue white pixel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    /// <summary>
    /// The red green blue white pixel.
    /// </summary>
    internal class RedGreenBlueWhitePixel : RedGreenBluePixel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedGreenBlueWhitePixel"/> class.
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
        public RedGreenBlueWhitePixel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            this.WhiteChannel = white;
        }

        /// <summary>
        ///   Gets WhiteChannel.
        /// </summary>
        public Channel WhiteChannel { get; private set; }
    }
}