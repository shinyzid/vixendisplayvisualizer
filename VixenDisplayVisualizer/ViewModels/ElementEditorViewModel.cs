// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Vixen.PlugIns.VixenDisplayVisualizer.Pixels;

    public class ElementEditorViewModel : ViewModelBase
    {
        private DisplayElement _displayElement;
        private PixelMapping _currentPixelMapping;
        private string _mappedChannelType;

        public ElementEditorViewModel(IEnumerable<Channel> channels, DisplayElement displayElement)
            : this()
        {
            Channels = channels;
            _displayElement = displayElement;
            PixelMappings = new ObservableCollection<PixelMapping>(displayElement.PixelMappings);
        }

        public ElementEditorViewModel()
        {
        }

        public IEnumerable<Channel> Channels { get; private set; }

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

        public ObservableCollection<PixelMapping> PixelMappings { get; private set; }
    }
}