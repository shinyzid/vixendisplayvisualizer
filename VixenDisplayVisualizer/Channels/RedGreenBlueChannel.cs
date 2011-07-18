namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class RedGreenBlueChannel : IChannel, INotifyPropertyChanged
    {
        protected byte _blue;

        protected byte _green;

        protected byte _red;
        private Channel _blueChannel;

        private Color _channelColor;
        private Channel _greenChannel;
        private Channel _redChannel;

        public RedGreenBlueChannel(Channel red, Channel green, Channel blue)
        {
            _channelColor = Colors.Black;
            RedChannel = red;
            GreenChannel = green;
            BlueChannel = blue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Channel BlueChannel
        {
            get
            {
                return _blueChannel;
            }

            set
            {
                _blueChannel = value;
                PropertyChanged.NotifyPropertyChanged("BlueChannel", this);
            }
        }

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

        public Channel GreenChannel
        {
            get
            {
                return _greenChannel;
            }

            set
            {
                _greenChannel = value;
                PropertyChanged.NotifyPropertyChanged("GreenChannel", this);
            }
        }

        public Channel RedChannel
        {
            get
            {
                return _redChannel;
            }

            set
            {
                _redChannel = value;
                PropertyChanged.NotifyPropertyChanged("RedChannel", this);
            }
        }

        public virtual bool Contains(Channel channel)
        {
            if (channel == null)
            {
                return false;
            }

            var id = channel.ID;
            var redChannel = RedChannel;
            var greenChannel = GreenChannel;
            var blueChannel = BlueChannel;
            return (redChannel != null && redChannel.ID == id) || (greenChannel != null && greenChannel.ID == id)
                   || (blueChannel != null && blueChannel.ID == id);
        }

        public virtual void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            var redChannel = RedChannel;
            if (redChannel != null
                && channelId == redChannel.ID)
            {
                _red = intensity;
            }
            else
            {
                var greenChannel = GreenChannel;
                if (greenChannel != null
                    && channelId == greenChannel.ID)
                {
                    _green = intensity;
                }
                else
                {
                    var blueChannel = BlueChannel;
                    if (blueChannel != null
                        && channelId == blueChannel.ID)
                    {
                        _blue = intensity;
                    }
                }
            }

            ChannelColor = Color.FromRgb(_red, _green, _blue);
        }
    }
}
