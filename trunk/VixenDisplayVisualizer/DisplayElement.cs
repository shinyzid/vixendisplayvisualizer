// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    /// <summary>
    /// The display element.
    /// </summary>
    public class DisplayElement
    {
        /// <summary>
        /// Gets Columns.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// Gets Rows.
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// Gets Height.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets LeftOffset.
        /// </summary>
        public int LeftOffset { get; private set; }

        /// <summary>
        /// Gets TopOffset.
        /// </summary>
        public int TopOffset { get; private set; }

        /// <summary>
        /// Gets Width.
        /// </summary>
        public int Width { get; private set; }

        public IList<MappedChannel> MappedChannels { get; private set; }
    }
}