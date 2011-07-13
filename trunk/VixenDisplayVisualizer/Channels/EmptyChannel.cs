namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Drawing;

    public class EmptyChannel : IChannel
    {
        public Color ChannelColor
        {
            get
            {
                return Color.Black;
            }
        }

        public bool Contains(Channel channel)
        {
            return false;
        }

        public void SetColor(Channel channel, byte intensity)
        {
            // Does nothing
        }
    }
}
