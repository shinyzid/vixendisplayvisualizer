namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    public class MappedChannel
    {
        public MappedChannel(IChannel channel)
        {
            Channel = channel;
        }

        public IChannel Channel { get; set; }

        public Color ChannelColor
        {
            get
            {
                return Channel.ChannelColor;
            }
        }

        public int Column { get; set; }

        public string Name { get; set; }

        public int Row { get; set; }

        public bool Contains(Channel channel)
        {
            return Channel == null ? false : Channel.Contains(channel);
        }

        public void SetColor(Channel channel, byte intensity)
        {
            Channel.SetColor(channel, intensity);
        }
    }
}
