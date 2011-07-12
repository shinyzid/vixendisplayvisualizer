namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System;
    using System.Drawing;

    internal class RedGreenBlueWhiteChannel : RedGreenBlueChannel
    {
        public RedGreenBlueWhiteChannel(Channel red, Channel green, Channel blue, Channel white)
            : base(red, green, blue)
        {
            WhiteChannel = white;
        }

        public override Color ChannelColor { get; protected set; }

        public Channel WhiteChannel { get; private set; }

        public override bool Contains(Channel channel)
        {
            return base.Contains(channel) || WhiteChannel.ID == channel.ID;
        }

        public override void SetColor(Channel channel, byte intensity)
        {
            throw new NotImplementedException();
        }
    }
}
