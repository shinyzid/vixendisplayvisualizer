// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Vixen.PlugIns.VixenDisplayVisualizer.Pixels;

    /// <summary>
    ///   The display element.
    /// </summary>
    public class DisplayElement : INotifyPropertyChanged
    {
        /// <summary>
        ///   The _columns.
        /// </summary>
        private int _columns;

        /// <summary>
        ///   The _height.
        /// </summary>
        private int _height;

        /// <summary>
        ///   The _left offset.
        /// </summary>
        private int _leftOffset;

        /// <summary>
        ///   The _name.
        /// </summary>
        private string _name;

        /// <summary>
        ///   The _rows.
        /// </summary>
        private int _rows;

        /// <summary>
        ///   The _top offset.
        /// </summary>
        private int _topOffset;

        /// <summary>
        ///   The _width.
        /// </summary>
        private int _width;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisplayElement" /> class.
        /// </summary>
        /// <param name = "columns">
        ///   The columns.
        /// </param>
        /// <param name = "rows">
        ///   The rows.
        /// </param>
        /// <param name = "height">
        ///   The height.
        /// </param>
        /// <param name = "leftOffset">
        ///   The left offset.
        /// </param>
        /// <param name = "topOffset">
        ///   The top offset.
        /// </param>
        /// <param name = "width">
        ///   The width.
        /// </param>
        /// <param name = "mappedChannels">
        ///   The mapped channels.
        /// </param>
        public DisplayElement(
            int columns, 
            int rows, 
            int height, 
            int leftOffset, 
            int topOffset, 
            int width, 
            IList<PixelMapping> mappedChannels)
        {
            this.PixelMappings = new ObservableCollection<PixelMapping>(mappedChannels);
            this._columns = columns;
            this._rows = rows;
            this.Height = height;
            this.LeftOffset = leftOffset;
            this.TopOffset = topOffset;
            this.Width = width;
            var numberOfCells = rows * columns;
            for (var index = mappedChannels.Count; index < numberOfCells; index++)
            {
                this.PixelMappings.Add(new PixelMapping(new EmptyPixel()));
            }
        }

        /// <summary>
        ///   The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Gets or sets Columns.
        /// </summary>
        public int Columns
        {
            get
            {
                return this._columns;
            }

            set
            {
                this._columns = value;
                this.PropertyChanged.NotifyPropertyChanged("Columns", this);
                this.AdjustMappedChannels();
            }
        }

        /// <summary>
        ///   Gets or sets Height.
        /// </summary>
        public int Height
        {
            get
            {
                return this._height;
            }

            set
            {
                this._height = value;
                this.PropertyChanged.NotifyPropertyChanged("Height", this);
            }
        }

        /// <summary>
        ///   Gets or sets LeftOffset.
        /// </summary>
        public int LeftOffset
        {
            get
            {
                return this._leftOffset;
            }

            set
            {
                this._leftOffset = value;
                this.PropertyChanged.NotifyPropertyChanged("LeftOffset", this);
            }
        }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
                this.PropertyChanged.NotifyPropertyChanged("Name", this);
            }
        }

        /// <summary>
        ///   Gets PixelMappings.
        /// </summary>
        public ObservableCollection<PixelMapping> PixelMappings { get; private set; }

        /// <summary>
        ///   Gets or sets Rows.
        /// </summary>
        public int Rows
        {
            get
            {
                return this._rows;
            }

            set
            {
                this._rows = value;
                this.PropertyChanged.NotifyPropertyChanged("Rows", this);
                this.AdjustMappedChannels();
            }
        }

        /// <summary>
        ///   Gets or sets TopOffset.
        /// </summary>
        public int TopOffset
        {
            get
            {
                return this._topOffset;
            }

            set
            {
                this._topOffset = value;
                this.PropertyChanged.NotifyPropertyChanged("TopOffset", this);
            }
        }

        /// <summary>
        ///   Gets or sets Width.
        /// </summary>
        public int Width
        {
            get
            {
                return this._width;
            }

            set
            {
                this._width = value;
                this.PropertyChanged.NotifyPropertyChanged("Width", this);
            }
        }

        /// <summary>
        ///   The adjust mapped channels.
        /// </summary>
        private void AdjustMappedChannels()
        {
            var numberOfCells = this._rows * this._columns;
            while (this.PixelMappings.Count > numberOfCells)
            {
                this.PixelMappings.RemoveAt(this.PixelMappings.Count - 1);
            }

            while (this.PixelMappings.Count < numberOfCells)
            {
                this.PixelMappings.Add(new PixelMapping(new EmptyPixel()));
            }
        }
    }
}