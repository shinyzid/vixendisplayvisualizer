namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class SingleColorChannel : IChannel, INotifyPropertyChanged
    {
        private Channel _channel;
        private Color _displayColor;

        public SingleColorChannel(Channel channel, Color color)
        {
            Channel = channel;
            ChannelColor = Colors.Black;
            DisplayColor = color;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Channel Channel
        {
            get
            {
                return _channel;
            }

            set
            {
                _channel = value;
                PropertyChanged.NotifyPropertyChanged("Channel", this);
            }
        }

        public Color ChannelColor { get; private set; }

        public Color DisplayColor
        {
            get
            {
                return _displayColor;
            }

            set
            {
                _displayColor = value;
                PropertyChanged.NotifyPropertyChanged("DisplayColor", this);
            }
        }

        public bool Contains(Channel channel)
        {
            var thisChannel = Channel;
            return thisChannel == null || channel == null ? false : thisChannel.ID == channel.ID;
        }

        public void SetColor(Channel channel, byte intensity)
        {
            ChannelColor = Color.FromArgb(intensity, DisplayColor.R, DisplayColor.G, DisplayColor.B);
        }
    }
}
