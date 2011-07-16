// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Windows.Media;

    internal class RedGreenBlueWhiteChannel : RedGreenBlueChannel
    {
        private byte _white;

        public RedGreenBlueWhiteChannel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            this.WhiteChannel = white;
        }

        public Channel WhiteChannel { get; set; }

        public override bool Contains(Channel channel)
        {
            return this.WhiteChannel != null && channel != null
                   && (base.Contains(channel) || this.WhiteChannel.ID == channel.ID);
        }

        public override void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            var halfIntensity = (byte)(intensity / 2);
            if (this.RedChannel != null && channelId == this.RedChannel.ID)
            {
                this._red = halfIntensity;
            }
            else if (this.GreenChannel != null && channelId == this.GreenChannel.ID)
            {
                this._green = halfIntensity;
            }
            else if (this.BlueChannel != null && channelId == this.BlueChannel.ID)
            {
                this._blue = halfIntensity;
            }
            else if (this.WhiteChannel != null && channelId == this.WhiteChannel.ID)
            {
                this._white = halfIntensity;
            }

            var red = (byte)(this._red + this._white);
            var green = (byte)(this._green + this._white);
            var blue = (byte)(this._blue + this._white);
            this.ChannelColor = Color.FromRgb(red, green, blue);
        }
    }
}