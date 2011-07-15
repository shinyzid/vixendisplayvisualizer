namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Media;

    public class MappedChannel : INotifyPropertyChanged
    {
        private IChannel _channel;

        public MappedChannel(IChannel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }

            Channel = channel;
            ConvertToEmptyCommand = new RelayCommand(x => ConvertToEmpty(), x => CanConvertToEmpty());
            ConvertToSingleCommand = new RelayCommand(x => ConvertToSingle(), x => CanConvertToSingle());
            ConvertToRgbCommand = new RelayCommand(x => ConvertToRgb(), x => CanConvertToRgb());
            ConvertToRgbwCommand = new RelayCommand(x => ConvertToRgbw(), x => CanConvertToRgbw());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IChannel Channel
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

        public Color ChannelColor
        {
            get
            {
                var channel = Channel;
                return channel == null ? Colors.Black : channel.ChannelColor;
            }
        }

        public ICommand ConvertToEmptyCommand { get; private set; }

        public ICommand ConvertToRgbCommand { get; private set; }

        public ICommand ConvertToRgbwCommand { get; private set; }
        public ICommand ConvertToSingleCommand { get; private set; }

        public bool Contains(Channel channel)
        {
            return Channel == null ? false : Channel.Contains(channel);
        }

        public void SetColor(Channel channel, byte intensity)
        {
            Channel.SetColor(channel, intensity);
            PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
        }

        private bool CanConvertToEmpty()
        {
            return !(Channel is EmptyChannel);
        }

        private bool CanConvertToRgb()
        {
            var rgb = Channel as RedGreenBlueChannel;
            return rgb == null || !(rgb is RedGreenBlueWhiteChannel);
        }

        private bool CanConvertToRgbw()
        {
            return !(Channel is RedGreenBlueWhiteChannel);
        }

        private bool CanConvertToSingle()
        {
            return !(Channel is SingleColorChannel);
        }

        private void ConvertToEmpty()
        {
            Channel = new EmptyChannel();
        }

        private void ConvertToRgb()
        {
            Channel = new RedGreenBlueChannel(null, null, null);
        }

        private void ConvertToRgbw()
        {
            Channel = new RedGreenBlueWhiteChannel(null, null, null, null);
        }

        private void ConvertToSingle()
        {
            Channel = new SingleColorChannel(null, Colors.HotPink);
        }
    }
}
