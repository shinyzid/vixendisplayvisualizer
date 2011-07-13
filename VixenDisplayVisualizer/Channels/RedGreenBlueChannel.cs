namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System;
    using System.Windows.Media;

    public class RedGreenBlueChannel : IChannel
    {
        public RedGreenBlueChannel(Channel red, Channel green, Channel blue)
        {
            RedChannel = red;
            GreenChannel = green;
            BlueChannel = blue;
        }

        public Channel BlueChannel { get; set; }

        public virtual Color ChannelColor { get; protected set; }

        public Channel GreenChannel { get; set; }

        public Channel RedChannel { get; set; }

        public virtual bool Contains(Channel channel)
        {
            var id = channel.ID;
            return RedChannel.ID == id || GreenChannel.ID == id || BlueChannel.ID == id;
        }

        public virtual void SetColor(Channel channel, byte intensity)
        {            
            throw new NotImplementedException();
        }
    }
}
