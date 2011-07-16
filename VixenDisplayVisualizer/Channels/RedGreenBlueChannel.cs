// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class RedGreenBlueChannel : IChannel, INotifyPropertyChanged
    {
        protected byte _blue;

        protected byte _green;

        protected byte _red;

        private Color _channelColor;

        public RedGreenBlueChannel(Channel red, Channel green, Channel blue)
        {
            this._channelColor = Colors.Black;
            this.RedChannel = red;
            this.GreenChannel = green;
            this.BlueChannel = blue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Channel BlueChannel { get; set; }

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

        public Channel GreenChannel { get; set; }

        public Channel RedChannel { get; set; }

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
            return (redChannel != null && redChannel.ID == id) 
                || (greenChannel != null && greenChannel.ID == id)
                || (blueChannel != null && blueChannel.ID == id);
        }

        public virtual void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            if (this.RedChannel != null && channelId == this.RedChannel.ID)
            {
                this._red = intensity;
            }
            else if (this.GreenChannel != null && channelId == this.GreenChannel.ID)
            {
                this._green = intensity;
            }
            else if (this.BlueChannel != null && channelId == this.BlueChannel.ID)
            {
                this._blue = intensity;
            }

            this.ChannelColor = Color.FromRgb(this._red, this._green, this._blue);
        }
    }
}