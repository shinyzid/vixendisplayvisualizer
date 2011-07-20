// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    ///   The single color pixel.
    /// </summary>
    public class SingleColorPixel : IPixel, INotifyPropertyChanged
    {
        /// <summary>
        ///   The _channel.
        /// </summary>
        private Channel _channel;

        /// <summary>
        ///   The _display color.
        /// </summary>
        private Color _displayColor;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SingleColorPixel" /> class.
        /// </summary>
        /// <param name = "channel">
        ///   The channel.
        /// </param>
        /// <param name = "color">
        ///   The color.
        /// </param>
        public SingleColorPixel(Channel channel, Color color)
        {
            this.Channel = channel;
            this.ChannelColor = Colors.Transparent;
            this.DisplayColor = color;
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Gets or sets Channel.
        /// </summary>
        public Channel Channel
        {
            get
            {
                return this._channel;
            }

            set
            {
                this._channel = value;
                this.PropertyChanged.NotifyPropertyChanged("Channel", this);
            }
        }

        /// <summary>
        ///   Gets ChannelColor.
        /// </summary>
        public Color ChannelColor { get; private set; }

        /// <summary>
        ///   Gets or sets DisplayColor.
        /// </summary>
        public Color DisplayColor
        {
            get
            {
                return this._displayColor;
            }

            set
            {
                this._displayColor = value;
                this.PropertyChanged.NotifyPropertyChanged("DisplayColor", this);
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
            var thisChannel = this.Channel;
            return thisChannel == null || channel == null ? false : thisChannel.ID == channel.ID;
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
            this.ChannelColor = Color.FromArgb(intensity, this.DisplayColor.R, this.DisplayColor.G, this.DisplayColor.B);
        }
    }
}