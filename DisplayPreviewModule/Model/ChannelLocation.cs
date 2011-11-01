namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Vixen.Sys;

    [DataContract]
    public class ChannelLocation
    {
        private string _channelName;
        [DataMember]
        public double TopOffset { get; set; }

        [DataMember]
        public double LeftOffset { get; set; }

        [DataMember]
        public Guid ChannelId { get; set; }

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
    }
}