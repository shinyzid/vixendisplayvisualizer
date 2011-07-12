// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public class DisplayElement
    {
        public DisplayElement(int columns, int rows, int height, int leftOffset, int topOffset, int width, IList<MappedChannel> mappedChannels)
        {
            Columns = columns;
            Rows = rows;
            Height = height;
            LeftOffset = leftOffset;
            TopOffset = topOffset;
            Width = width;
            MappedChannels = mappedChannels;
        }

        public int Columns { get; private set; }

        public int Rows { get; private set; }

        public int Height { get; private set; }

        public int LeftOffset { get; private set; }

        public int TopOffset { get; private set; }

        public int Width { get; private set; }

        public IList<MappedChannel> MappedChannels { get; private set; }
    }
}