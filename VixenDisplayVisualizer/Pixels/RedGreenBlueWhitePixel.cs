namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.ComponentModel;
    using System.Windows.Media;

    internal class RedGreenBlueWhitePixel : RedGreenBluePixel
    {
        private byte _white;
        private Channel _whiteChannel;

        public RedGreenBlueWhitePixel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            WhiteChannel = white;
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public Channel WhiteChannel
        {
            get
            {
                return _whiteChannel;
            }

            set
            {
                _whiteChannel = value;
                PropertyChanged.NotifyPropertyChanged("WhiteChannel", this);
            }
        }

        public override bool Contains(Channel channel)
        {
            var whiteChannel = WhiteChannel;
            return channel != null && (base.Contains(channel) || (whiteChannel != null && whiteChannel.ID == channel.ID));
        }

        public override void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            var halfIntensity = (byte)(intensity / 2);
            var redChannel = RedChannel;
            if (redChannel != null
                && channelId == redChannel.ID)
            {
                _red = halfIntensity;
            }
            else
            {
                var greenChannel = GreenChannel;
                if (greenChannel != null
                    && channelId == greenChannel.ID)
                {
                    _green = halfIntensity;
                }
                else
                {
                    var blueChannel = BlueChannel;
                    if (blueChannel != null
                        && channelId == blueChannel.ID)
                    {
                        _blue = halfIntensity;
                    }
                    else
                    {
                        var whiteChannel = WhiteChannel;
                        if (whiteChannel != null
                            && channelId == whiteChannel.ID)
                        {
                            _white = halfIntensity;
                        }
                    }
                }
            }

            var red = (byte)(_red + _white);
            var green = (byte)(_green + _white);
            var blue = (byte)(_blue + _white);
            ChannelColor = Color.FromRgb(red, green, blue);
        }
    }
}
