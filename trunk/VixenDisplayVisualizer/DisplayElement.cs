// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public class DisplayElement : INotifyPropertyChanged
    {
        private int columns;

        private int height;

        private int leftOffset;

        private string name;

        private int rows;

        private int topOffset;

        private int width;

        public DisplayElement(
            int columns, 
            int rows, 
            int height, 
            int leftOffset, 
            int topOffset, 
            int width, 
            IList<MappedChannel> mappedChannels)
        {
            this.Columns = columns;
            this.Rows = rows;
            this.Height = height;
            this.LeftOffset = leftOffset;
            this.TopOffset = topOffset;
            this.Width = width;
            this.MappedChannels = mappedChannels;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Columns
        {
            get
            {
                return this.columns;
            }

            set
            {
                this.columns = value;
                this.PropertyChanged.NotifyPropertyChanged("Columns", this);
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
                this.PropertyChanged.NotifyPropertyChanged("Height", this);
            }
        }

        public int LeftOffset
        {
            get
            {
                return this.leftOffset;
            }

            set
            {
                this.leftOffset = value;
                this.PropertyChanged.NotifyPropertyChanged("LeftOffset", this);
            }
        }

        public IList<MappedChannel> MappedChannels { get; private set; }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.PropertyChanged.NotifyPropertyChanged("Name", this);
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
                this.PropertyChanged.NotifyPropertyChanged("Rows", this);
            }
        }

        public int TopOffset
        {
            get
            {
                return this.topOffset;
            }

            set
            {
                this.topOffset = value;
                this.PropertyChanged.NotifyPropertyChanged("TopOffset", this);
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
                this.PropertyChanged.NotifyPropertyChanged("Width", this);
            }
        }
    }
}