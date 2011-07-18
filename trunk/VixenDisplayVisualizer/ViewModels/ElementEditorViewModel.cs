// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public class ElementEditorViewModel : ViewModelBase
    {
        private DisplayElement _displayElement;

        private MappedChannel _currentMappedChannel;
        private string _mappedChannelType;

        public ElementEditorViewModel(IEnumerable<Channel> channels, DisplayElement displayElement)
            : this()
        {
            Channels = channels;
            _displayElement = displayElement;
            MappedChannels = new ObservableCollection<MappedChannel>(displayElement.MappedChannels);
        }

        public ElementEditorViewModel()
        {
        }

        public IEnumerable<Channel> Channels { get; private set; }

        public MappedChannel CurrentMappedChannel
        {
            get
            {
                return this._currentMappedChannel;
            }

            set
            {
                this._currentMappedChannel = value;
                this.OnPropertyChanged("CurrentMappedChannel");
                UpdateMappedChannelType();
            }
        }

        private void UpdateMappedChannelType()
        {
            var channel = _currentMappedChannel.Channel;
            if (channel is RedGreenBlueWhiteChannel)
            {
                MappedChannelType = "RGB+W";
            }
            else if (channel is RedGreenBlueChannel)
            {
                MappedChannelType = "RGB";
            }
            else if (channel is SingleColorChannel)
            {
                MappedChannelType = "Single";
            }
            else
            {
                MappedChannelType = "None";
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

        public string MappedChannelType
        {
            get
            {
                return _mappedChannelType;
            }

            set
            {
                _mappedChannelType = value;
                OnPropertyChanged("MappedChannelType");
            }
        }

        public ObservableCollection<MappedChannel> MappedChannels { get; private set; }
    }
}