namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class RedGreenBlueChannel : IChannel, INotifyPropertyChanged
    {
        protected Color _blue = Color.FromArgb(0, 0, 0, 0xFF);
        protected Color _green = Color.FromArgb(0, 0, 0xFF, 0);
        protected Color _red = Color.FromArgb(0, 0xFF, 0, 0);
        private Color _channelColor;

        public RedGreenBlueChannel(Channel red, Channel green, Channel blue)
        {
            _channelColor = Colors.Black;
            RedChannel = red;
            GreenChannel = green;
            BlueChannel = blue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Channel BlueChannel { get; set; }

        public virtual Color ChannelColor
        {
            get
            {
                return _channelColor;
            }

            protected set
            {
                _channelColor = value;
                PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
            }
        }

        public Channel GreenChannel { get; set; }

        public Channel RedChannel { get; set; }

        public virtual bool Contains(Channel channel)
        {
            var id = channel.ID;
            return RedChannel.ID == id || GreenChannel.ID == id || BlueChannel.ID == id;
        }

        public virtual void SetColor(Channel channel, byte intensity)
        {
            var channelId = channel.ID;
            if (RedChannel != null
                && channelId == RedChannel.ID)
            {
                _red = Color.FromArgb(intensity, 0xFF, 0, 0);
            }
            else if (GreenChannel != null
                     && channelId == GreenChannel.ID)
            {
                _green = Color.FromArgb(intensity, 0, 0xFF, 0);
            }
            else if (BlueChannel != null
                     && channelId == BlueChannel.ID)
            {
                _blue = Color.FromArgb(intensity, 0, 0, 0xFF);
            }

            ChannelColor = _red + _green + _blue;
        }
    }
}
