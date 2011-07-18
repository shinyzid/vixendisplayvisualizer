// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.Windows.Media;

    public interface IPixel
    {
        Color ChannelColor { get; }

        bool Contains(Channel channel);

        void SetColor(Channel channel, byte intensity);
    }
}