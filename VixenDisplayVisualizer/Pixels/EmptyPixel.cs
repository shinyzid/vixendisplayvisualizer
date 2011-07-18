namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.Windows.Media;

    public class EmptyPixel : IPixel
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
