namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Windows.Media;

    public class EmptyChannel : IChannel
    {
        public Color ChannelColor
        {
            get
            {
                return Colors.Black;
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
