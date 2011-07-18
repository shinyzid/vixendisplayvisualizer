namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Media;

    public class PixelMapping : INotifyPropertyChanged
    {
        private IPixel _pixel;

        public PixelMapping(IPixel pixel)
        {
            if (pixel == null)
            {
                throw new ArgumentNullException("pixel");
            }

            Pixel = pixel;
            ConvertToEmptyCommand = new RelayCommand(x => ConvertToEmpty(), x => CanConvertToEmpty());
            ConvertToSingleCommand = new RelayCommand(x => ConvertToSingle(), x => CanConvertToSingle());
            ConvertToRgbCommand = new RelayCommand(x => ConvertToRgb(), x => CanConvertToRgb());
            ConvertToRgbwCommand = new RelayCommand(x => ConvertToRgbw(), x => CanConvertToRgbw());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IPixel Pixel
        {
            get
            {
                return _pixel;
            }

            set
            {
                _pixel = value;
                PropertyChanged.NotifyPropertyChanged("Pixel", this);
            }
        }

        public Color ChannelColor
        {
            get
            {
                var channel = Pixel;
                return channel == null ? Colors.Black : channel.ChannelColor;
            }
        }

        public ICommand ConvertToEmptyCommand { get; private set; }

        public ICommand ConvertToRgbCommand { get; private set; }

        public ICommand ConvertToRgbwCommand { get; private set; }

        public ICommand ConvertToSingleCommand { get; private set; }

        public bool Contains(Channel channel)
        {
            return Pixel == null ? false : Pixel.Contains(channel);
        }

        public void SetColor(Channel channel, byte intensity)
        {
            Pixel.SetColor(channel, intensity);
            PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
        }

        private bool CanConvertToEmpty()
        {
            return !(Pixel is EmptyPixel);
        }

        private bool CanConvertToRgb()
        {
            var rgb = Pixel as RedGreenBluePixel;
            return rgb == null || !(rgb is RedGreenBlueWhitePixel);
        }

        private bool CanConvertToRgbw()
        {
            return !(Pixel is RedGreenBlueWhitePixel);
        }

        private bool CanConvertToSingle()
        {
            return !(Pixel is SingleColorPixel);
        }

        private void ConvertToEmpty()
        {
            Pixel = new EmptyPixel();
        }

        private void ConvertToRgb()
        {
            Pixel = new RedGreenBluePixel(null, null, null);
        }

        private void ConvertToRgbw()
        {
            Pixel = new RedGreenBlueWhitePixel(null, null, null, null);
        }

        private void ConvertToSingle()
        {
            Pixel = new SingleColorPixel(null, Colors.White);
        }
    }
}
