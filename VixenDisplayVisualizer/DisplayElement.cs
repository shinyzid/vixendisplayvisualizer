namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public class DisplayElement : INotifyPropertyChanged
    {
        private int _columns;

        private int _height;

        private int _leftOffset;

        private string _name;

        private int _rows;

        private int _topOffset;

        private int _width;

        public DisplayElement(
            int columns, int rows, int height, int leftOffset, int topOffset, int width, IList<MappedChannel> mappedChannels)
        {
            MappedChannels = new ObservableCollection<MappedChannel>(mappedChannels);
            _columns = columns;
            _rows = rows;
            Height = height;
            LeftOffset = leftOffset;
            TopOffset = topOffset;
            Width = width;
            var numberOfCells = rows * columns;           
            for (var index = mappedChannels.Count; index < numberOfCells; index++)
            {
                MappedChannels.Add(new MappedChannel(new EmptyChannel()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Columns
        {
            get
            {
                return _columns;
            }

            set
            {
                _columns = value;
                PropertyChanged.NotifyPropertyChanged("Columns", this);
                AdjustMappedChannels();
            }
        }

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

        public ObservableCollection<MappedChannel> MappedChannels { get; private set; }

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

        public int Rows
        {
            get
            {
                return _rows;
            }

            set
            {
                _rows = value;
                PropertyChanged.NotifyPropertyChanged("Rows", this);
                AdjustMappedChannels();
            }
        }

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

        private void AdjustMappedChannels()
        {
            var numberOfCells = _rows * _columns;
            while (MappedChannels.Count > numberOfCells)
            {
                MappedChannels.RemoveAt(MappedChannels.Count - 1);
            }

            while (MappedChannels.Count < numberOfCells)
            {
                MappedChannels.Add(new MappedChannel(new EmptyChannel()));
            }
        }
    }
}
