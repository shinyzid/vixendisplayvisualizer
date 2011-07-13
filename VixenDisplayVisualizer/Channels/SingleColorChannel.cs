// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Windows.Media;

    public class SingleColorChannel : IChannel
    {
        public SingleColorChannel(Channel channel, Color color)
        {
            Channel = channel;
            ChannelColor = Colors.Transparent;
            DisplayColor = color;
        }

        public Channel Channel { get; set; }

        public Color ChannelColor { get; private set; }

        public Color DisplayColor { get; private set; }

        public bool Contains(Channel channel)
        {
            return Channel.ID == channel.ID;
        }

        public void SetColor(Channel channel, byte intensity)
        {
            ChannelColor = Color.FromArgb(intensity, DisplayColor.R, DisplayColor.G, DisplayColor.B);
        }
    }
}