// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    ///   The pixel mapping.
    /// </summary>
    public class PixelMapping : INotifyPropertyChanged
    {
        /// <summary>
        ///   The _pixel.
        /// </summary>
        private IPixel _pixel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PixelMapping" /> class.
        /// </summary>
        /// <param name = "pixel">
        ///   The pixel.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        public PixelMapping(IPixel pixel)
        {
            if (pixel == null)
            {
                throw new ArgumentNullException("pixel");
            }

            this.Pixel = pixel;
            this.ConvertToEmptyCommand = new RelayCommand(x => this.ConvertToEmpty(), x => this.CanConvertToEmpty());
            this.ConvertToSingleCommand = new RelayCommand(x => this.ConvertToSingle(), x => this.CanConvertToSingle());
            this.ConvertToRgbCommand = new RelayCommand(x => this.ConvertToRgb(), x => this.CanConvertToRgb());
            this.ConvertToRgbwCommand = new RelayCommand(x => this.ConvertToRgbw(), x => this.CanConvertToRgbw());
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Gets ChannelColor.
        /// </summary>
        public Color ChannelColor
        {
            get
            {
                var channel = this.Pixel;
                return channel == null ? Colors.Black : channel.ChannelColor;
            }
        }

        /// <summary>
        ///   Gets ConvertToEmptyCommand.
        /// </summary>
        public ICommand ConvertToEmptyCommand { get; private set; }

        /// <summary>
        ///   Gets ConvertToRgbCommand.
        /// </summary>
        public ICommand ConvertToRgbCommand { get; private set; }

        /// <summary>
        ///   Gets ConvertToRgbwCommand.
        /// </summary>
        public ICommand ConvertToRgbwCommand { get; private set; }

        /// <summary>
        ///   Gets ConvertToSingleCommand.
        /// </summary>
        public ICommand ConvertToSingleCommand { get; private set; }

        /// <summary>
        ///   Gets or sets Pixel.
        /// </summary>
        public IPixel Pixel
        {
            get
            {
                return this._pixel;
            }

            set
            {
                this._pixel = value;
                this.PropertyChanged.NotifyPropertyChanged("Pixel", this);
            }
        }

        /// <summary>
        ///   The contains.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <returns>
        ///   The contains.
        /// </returns>
        public bool Contains(Channel channel)
        {
            return this.Pixel == null ? false : this.Pixel.Contains(channel);
        }

        /// <summary>
        ///   The set color.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <param name = "intensity">
        ///   The intensity.
        /// </param>
        public void SetColor(Channel channel, byte intensity)
        {
            this.Pixel.SetColor(channel, intensity);
            this.PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
        }

        /// <summary>
        ///   The can convert to empty.
        /// </summary>
        /// <returns>
        ///   The can convert to empty.
        /// </returns>
        private bool CanConvertToEmpty()
        {
            return !(this.Pixel is EmptyPixel);
        }

        /// <summary>
        ///   The can convert to rgb.
        /// </summary>
        /// <returns>
        ///   The can convert to rgb.
        /// </returns>
        private bool CanConvertToRgb()
        {
            var rgb = this.Pixel as RedGreenBluePixel;
            return rgb == null || !(rgb is RedGreenBlueWhitePixel);
        }

        /// <summary>
        ///   The can convert to rgbw.
        /// </summary>
        /// <returns>
        ///   The can convert to rgbw.
        /// </returns>
        private bool CanConvertToRgbw()
        {
            return !(this.Pixel is RedGreenBlueWhitePixel);
        }

        /// <summary>
        ///   The can convert to single.
        /// </summary>
        /// <returns>
        ///   The can convert to single.
        /// </returns>
        private bool CanConvertToSingle()
        {
            return !(this.Pixel is SingleColorPixel);
        }

        /// <summary>
        ///   The convert to empty.
        /// </summary>
        private void ConvertToEmpty()
        {
            this.Pixel = new EmptyPixel();
        }

        /// <summary>
        ///   The convert to rgb.
        /// </summary>
        private void ConvertToRgb()
        {
            this.Pixel = new RedGreenBluePixel(null, null, null);
        }

        /// <summary>
        ///   The convert to rgbw.
        /// </summary>
        private void ConvertToRgbw()
        {
            this.Pixel = new RedGreenBlueWhitePixel(null, null, null, null);
        }

        /// <summary>
        ///   The convert to single.
        /// </summary>
        private void ConvertToSingle()
        {
            this.Pixel = new SingleColorPixel(null, Colors.White);
        }
    }
}