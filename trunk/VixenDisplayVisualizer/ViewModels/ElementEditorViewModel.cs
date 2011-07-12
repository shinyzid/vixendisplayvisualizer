namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    public class ElementEditorViewModel : ViewModelBase
    {
        private DisplayElement _displayElement;

        public ElementEditorViewModel(IEnumerable<Channel> channels, DisplayElement displayElement)
        {
            AddMappedChannelCommand = new RelayCommand(x => AddMappedChannel());
            Channels = channels;
            _displayElement = displayElement;
            MappedChannels = displayElement.MappedChannels;
        }

        public ElementEditorViewModel() { }

        public ICommand AddMappedChannelCommand { get; private set; }

        public IEnumerable<Channel> Channels { get; private set; }

        public IEnumerable<MappedChannel> MappedChannels { get; private set; }

        public MappedChannel CurrentMappedChannel { get; set; }

        private void AddMappedChannel()
        {
            var mappedChannel = new MappedChannel(null);
            var viewModel = new MappedChannelEditorViewModel(Channels, mappedChannel);
            using (var mapped = new MappedChannelEditor(viewModel))
            {
                if (mapped.ShowDialog() == DialogResult.OK)
                {
                    _displayElement.MappedChannels.Add(mappedChannel);
                    MappedChannels = _displayElement.MappedChannels;
                }
            }
        }
    }
}
