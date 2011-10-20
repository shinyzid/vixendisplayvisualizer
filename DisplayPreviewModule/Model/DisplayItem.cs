namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Vixen.Sys;

    public class DisplayItem : INotifyPropertyChanged
    {
        private int _height;
        private bool _isUnlocked;
        private int _leftOffset;
        private string _name;
        private int _topOffset;
        private int _width;

        public DisplayItem(int width, int height, int leftOffset, int topOffset, IList<ChannelLocation> mappedChannels, bool isUnlocked, IList<Channel> availableChannels)
        {
            ChannelLocations = new ObservableCollection<ChannelLocation>(mappedChannels);
            Height = height;
            LeftOffset = leftOffset;
            TopOffset = topOffset;
            Width = width;
            IsUnlocked = isUnlocked;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public bool IsUnlocked
        {
            get
            {
                return _isUnlocked;
            }

            set
            {
                _isUnlocked = value;
                PropertyChanged.NotifyPropertyChanged("IsUnlocked", this);
            }
        }

        public int LeftOffset
        {
            get
            {
                return _leftOffset;
            }

            set
            {
                _leftOffset = value;
                PropertyChanged.NotifyPropertyChanged("LeftOffset", this);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                PropertyChanged.NotifyPropertyChanged("Name", this);
            }
        }

        public ObservableCollection<ChannelLocation> ChannelLocations { get; private set; }

        public int TopOffset
        {
            get
            {
                return _topOffset;
            }

            set
            {
                _topOffset = value;
                PropertyChanged.NotifyPropertyChanged("TopOffset", this);
            }
        }

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
    }
}
