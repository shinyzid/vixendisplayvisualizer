namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.Windows.Media;

    internal class RedGreenBlueWhiteChannel : RedGreenBlueChannel
    {
        private Color _white = Color.FromArgb(0, 0xFF, 0xFF, 0xFF);

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
            var channelId = channel.ID;
            if (RedChannel != null && channelId == RedChannel.ID)
            {
                _red = Color.FromArgb(intensity, 0xFF, 0, 0);
            }
            else if (GreenChannel != null && channelId == GreenChannel.ID)
            {
                _green = Color.FromArgb(intensity, 0, 0xFF, 0);
            }
            else if (BlueChannel != null && channelId == BlueChannel.ID)
            {
                _blue = Color.FromArgb(intensity, 0, 0, 0xFF);
            }
            else if (WhiteChannel != null && channelId == WhiteChannel.ID)
            {
                _white = Color.FromArgb(intensity, 0xFF, 0xFF, 0xFF);
            }

            ChannelColor = _red + _green + _blue + _white;
        }
    }
}
