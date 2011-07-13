// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    public class ElementEditorViewModel : ViewModelBase
    {
        private DisplayElement _displayElement;

        private MappedChannel _currentMappedChannel;

        public ElementEditorViewModel(IEnumerable<Channel> channels, DisplayElement displayElement)
            : this()
        {
            this.Channels = channels;
            this._displayElement = displayElement;
            this.MappedChannels = new ObservableCollection<MappedChannel>(displayElement.MappedChannels);
        }

        public ElementEditorViewModel()
        {
            this.AddMappedChannelCommand = new RelayCommand(x => this.AddMappedChannel());
        }

        public ICommand AddMappedChannelCommand { get; private set; }

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

        public ObservableCollection<MappedChannel> MappedChannels { get; private set; }

        private void AddMappedChannel()
        {
            var mappedChannel = new MappedChannel(null);
            var viewModel = new MappedChannelEditorViewModel(this.Channels, mappedChannel);
            using (var mapped = new MappedChannelEditor(viewModel))
            {
                mapped.ShowDialog();
                this._displayElement.MappedChannels.Add(mappedChannel);
                this.MappedChannels.Add(mappedChannel);
            }
        }
    }
}