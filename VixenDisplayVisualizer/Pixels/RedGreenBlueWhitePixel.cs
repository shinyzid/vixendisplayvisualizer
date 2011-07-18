// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Pixels
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    ///   The red green blue white pixel.
    /// </summary>
    internal class RedGreenBlueWhitePixel : RedGreenBluePixel
    {
        /// <summary>
        ///   The _white.
        /// </summary>
        private byte _white;

        /// <summary>
        ///   The _white channel.
        /// </summary>
        private Channel _whiteChannel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RedGreenBlueWhitePixel" /> class.
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
        /// <param name = "white">
        ///   The white.
        /// </param>
        public RedGreenBlueWhitePixel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            this.WhiteChannel = white;
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Gets or sets WhiteChannel.
        /// </summary>
        public Channel WhiteChannel
        {
            get
            {
                return this._whiteChannel;
            }

            set
            {
                this._whiteChannel = value;
                this.PropertyChanged.NotifyPropertyChanged("WhiteChannel", this);
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
        public override bool Contains(Channel channel)
        {
            var whiteChannel = this.WhiteChannel;
            return channel != null
                   && (base.Contains(channel) || (whiteChannel != null && whiteChannel.ID == channel.ID));
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
        public override void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            var halfIntensity = (byte)(intensity / 2);
            var redChannel = this.RedChannel;
            if (redChannel != null && channelId == redChannel.ID)
            {
                this._red = halfIntensity;
            }
            else
            {
                var greenChannel = this.GreenChannel;
                if (greenChannel != null && channelId == greenChannel.ID)
                {
                    this._green = halfIntensity;
                }
                else
                {
                    var blueChannel = this.BlueChannel;
                    if (blueChannel != null && channelId == blueChannel.ID)
                    {
                        this._blue = halfIntensity;
                    }
                    else
                    {
                        var whiteChannel = this.WhiteChannel;
                        if (whiteChannel != null && channelId == whiteChannel.ID)
                        {
                            this._white = halfIntensity;
                        }
                    }
                }
            }

            var red = (byte)(this._red + this._white);
            var green = (byte)(this._green + this._white);
            var blue = (byte)(this._blue + this._white);
            this.ChannelColor = Color.FromRgb(red, green, blue);
        }
    }
}