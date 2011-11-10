namespace VixenModules.App.DisplayPreview.Model
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Preferences
    {
        [DataMember]
        public bool KeepVisualizerWindowOpen { get; set; }

        [DataMember]
        public int MinDisplayWidth { get; set; }

        [DataMember]
        public int MinDisplayHeight { get; set; }

        [DataMember]
        public int MinDisplayItemWidth { get; set; }

        [DataMember]
        public int MinDisplayItemHeight { get; set; }

        [DataMember]
        public int MinChannelWidth { get; set; }

        [DataMember]
        public int MinChannelHeight { get; set; }

        [DataMember]
        public double DefaultOpacity { get; set; }
    }
}