namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Windows.Media;
    using Vixen.Sys;

    [DataContract]
    public class ChannelLocation : INotifyPropertyChanged
    {
        private Color _channelColor;
        private string _channelName;
        private int _height;
        private bool _isSelected;
        private int _width;

        public ChannelLocation()
        {
            Width = 10;
            Height = 10;
            ChannelColor = Colors.Black;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChannelNode Channel
        {
            get
            {
                return VixenSystem.Nodes.GetAllNodes().FirstOrDefault(x => x.Id == ChannelId);
            }
        }

        public Color ChannelColor
        {
            get
            {
                return _channelColor;
            }

            set
            {
                _channelColor = value;
                PropertyChanged.NotifyPropertyChanged("ChannelColor", this);
            }
        }

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

        [DataMember]
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
                PropertyChanged.NotifyPropertyChanged("Height", this);
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                PropertyChanged.NotifyPropertyChanged("IsSelected", this);
            }
        }

        [DataMember]
        public double LeftOffset { get; set; }

        [DataMember]
        public double TopOffset { get; set; }

        [DataMember]
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
                PropertyChanged.NotifyPropertyChanged("Width", this);
            }
        }

        public ChannelLocation Clone()
        {
            return new ChannelLocation { TopOffset = TopOffset, LeftOffset = LeftOffset, ChannelId = ChannelId };
        }
    }
}
