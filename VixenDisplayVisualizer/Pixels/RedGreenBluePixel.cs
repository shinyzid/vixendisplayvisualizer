// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    ///   The red green blue pixel.
    /// </summary>
    public class RedGreenBluePixel : IPixel, INotifyPropertyChanged
    {
        /// <summary>
        ///   The _blue.
        /// </summary>
        protected byte _blue;

        /// <summary>
        ///   The _green.
        /// </summary>
        protected byte _green;

        /// <summary>
        ///   The _red.
        /// </summary>
        protected byte _red;

        /// <summary>
        ///   The _blue channel.
        /// </summary>
        private Channel _blueChannel;

        /// <summary>
        ///   The _channel color.
        /// </summary>
        private Color _channelColor;

        /// <summary>
        ///   The _green channel.
        /// </summary>
        private Channel _greenChannel;

        /// <summary>
        ///   The _red channel.
        /// </summary>
        private Channel _redChannel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RedGreenBluePixel" /> class.
        /// </summary>
        /// <param name = "red">
        ///   The red.
        /// </param>
        /// <param name = "green">
        ///   The green.
        /// </param>
        /// <param name = "blue">
        ///   The blue.
        /// </param>
        public RedGreenBluePixel(Channel red, Channel green, Channel blue)
        {
            this._channelColor = Colors.Transparent;
            this.RedChannel = red;
            this.GreenChannel = green;
            this.BlueChannel = blue;
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Gets or sets BlueChannel.
        /// </summary>
        public Channel BlueChannel
        {
            get
            {
                return this._blueChannel;
            }

            set
            {
                this._blueChannel = value;
                this.PropertyChanged.NotifyPropertyChanged("BlueChannel", this);
            }
        }

        /// <summary>
        ///   Gets or sets ChannelColor.
        /// </summary>
        public virtual Color ChannelColor
        {
            get
            {
                return this._channelColor;
            }

            protected set
            {
                this._channelColor = value;
                this.PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
            }
        }

        /// <summary>
        ///   Gets or sets GreenChannel.
        /// </summary>
        public Channel GreenChannel
        {
            get
            {
                return this._greenChannel;
            }

            set
            {
                this._greenChannel = value;
                this.PropertyChanged.NotifyPropertyChanged("GreenChannel", this);
            }
        }

        /// <summary>
        ///   Gets or sets RedChannel.
        /// </summary>
        public Channel RedChannel
        {
            get
            {
                return this._redChannel;
            }

            set
            {
                this._redChannel = value;
                this.PropertyChanged.NotifyPropertyChanged("RedChannel", this);
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
        public virtual bool Contains(Channel channel)
        {
            if (channel == null)
            {
                return false;
            }

            var id = channel.ID;
            var redChannel = this.RedChannel;
            var greenChannel = this.GreenChannel;
            var blueChannel = this.BlueChannel;
            return (redChannel != null && redChannel.ID == id) || (greenChannel != null && greenChannel.ID == id)
                   || (blueChannel != null && blueChannel.ID == id);
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
        public virtual void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            var redChannel = this.RedChannel;
            if (redChannel != null && channelId == redChannel.ID)
            {
                this._red = intensity;
            }
            else
            {
                var greenChannel = this.GreenChannel;
                if (greenChannel != null && channelId == greenChannel.ID)
                {
                    this._green = intensity;
                }
                else
                {
                    var blueChannel = this.BlueChannel;
                    if (blueChannel != null && channelId == blueChannel.ID)
                    {
                        this._blue = intensity;
                    }
                }
            }

            this.ChannelColor = Color.FromRgb(this._red, this._green, this._blue);
        }
    }
}