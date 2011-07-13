// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using Color = System.Windows.Media.Color;

    public interface IChannel
    {
        Color ChannelColor { get; }

        bool Contains(Channel channel);

        void SetColor(Channel channel, byte intensity);
    }
}