namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ChannelLocation
    {
        [DataMember]
        public int TopOffset { get; set; }

        [DataMember]
        public int LeftOffset { get; set; }

        [DataMember]
        public Guid ChannelId { get; set; }
    }
}