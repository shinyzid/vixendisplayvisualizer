namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Windows.Media;
    using Vixen.Sys;

    [DataContract]
    public class ChannelLocation
    {
        private string _channelName;

        public ChannelLocation()
        {
            Width = 10;
            Height = 10;
            ChannelColor = Colors.Yellow;
        }

        [DataMember]
        public double TopOffset { get; set; }

        [DataMember]
        public double LeftOffset { get; set; }

        [DataMember]
        public Guid ChannelId { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        public Color ChannelColor { get; set; }

        public string ChannelName
        {
            get
            {
                if (_channelName == null)
                {
                    var channel = VixenSystem.Nodes.GetAllNodes().FirstOrDefault(x => x.Id == ChannelId);
                    if (channel != null)
                    {
                        _channelName = channel.Name;
                    }
                }

                return _channelName;
            }
        }

        public ChannelLocation Clone()
        {
            return new ChannelLocation { TopOffset = TopOffset, LeftOffset = LeftOffset, ChannelId = ChannelId };
        }
    }
}