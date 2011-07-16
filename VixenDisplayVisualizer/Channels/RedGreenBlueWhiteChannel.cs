namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Windows.Media;

    internal class RedGreenBlueWhiteChannel : RedGreenBlueChannel
    {
        private byte _white = 0x00;

        public RedGreenBlueWhiteChannel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            WhiteChannel = white;
        }

        public Channel WhiteChannel { get; set; }

        public override bool Contains(Channel channel)
        {
            return base.Contains(channel) || WhiteChannel.ID == channel.ID;
        }

        public override void SetColor(Channel channel, byte intensity)
        {
            if (channel == null)
            {
                return;
            }

            var channelId = channel.ID;
            if (RedChannel != null && channelId == RedChannel.ID)
            {
                _red = Color.FromRgb(intensity, 0, 0);
            }
            else if (GreenChannel != null && channelId == GreenChannel.ID)
            {
                _green = Color.FromRgb(0, intensity, 0);
            }
            else if (BlueChannel != null && channelId == BlueChannel.ID)
            {
                _blue = Color.FromRgb(0, 0, intensity);
            }
            else if (WhiteChannel != null && channelId == WhiteChannel.ID)
            {
                _white = (byte)(intensity / 2);
            }

            var red = (byte)((byte)(this._red.R / 2) + _white);
            var green = (byte)((byte)(this._green.G / 2) + _white);
            var blue = (byte)((byte)(this._blue.B / 2) + _white);
            ChannelColor = Color.FromRgb(red, green, blue);
        }
    }
}
