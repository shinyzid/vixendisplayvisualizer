// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Vixen.PlugIns.VixenDisplayVisualizer.Pixels;

    /// <summary>
    ///   The element editor view model.
    /// </summary>
    public class ElementEditorViewModel : ViewModelBase
    {
        /// <summary>
        ///   The _current pixel mapping.
        /// </summary>
        private PixelMapping _currentPixelMapping;

        /// <summary>
        ///   The _display element.
        /// </summary>
        private DisplayElement _displayElement;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ElementEditorViewModel" /> class.
        /// </summary>
        /// <param name = "channels">
        ///   The channels.
        /// </param>
        /// <param name = "displayElement">
        ///   The display element.
        /// </param>
        public ElementEditorViewModel(IEnumerable<Channel> channels, DisplayElement displayElement)
            : this()
        {
            this.Channels = channels;
            this._displayElement = displayElement;
            this.PixelMappings = new ObservableCollection<PixelMapping>(displayElement.PixelMappings);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ElementEditorViewModel" /> class.
        /// </summary>
        public ElementEditorViewModel()
        {
        }

        /// <summary>
        ///   Gets Channels.
        /// </summary>
        public IEnumerable<Channel> Channels { get; private set; }

        /// <summary>
        ///   Gets or sets CurrentPixelMapping.
        /// </summary>
        public PixelMapping CurrentPixelMapping
        {
            get
            {
                return this._currentPixelMapping;
            }

            set
            {
                this._currentPixelMapping = value;
                this.OnPropertyChanged("CurrentPixelMapping");
            }
        }

        /// <summary>
        ///   Gets or sets DisplayElement.
        /// </summary>
        public DisplayElement DisplayElement
        {
            get
            {
                return this._displayElement;
            }

            set
            {
                this._displayElement = value;
                this.OnPropertyChanged("DisplayElement");
            }
        }

        /// <summary>
        ///   Gets PixelMappings.
        /// </summary>
        public ObservableCollection<PixelMapping> PixelMappings { get; private set; }
    }
}