namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public class MappedChannelEditorViewModel : ViewModelBase
    {
        public MappedChannelEditorViewModel(IEnumerable<Channel> channels, MappedChannel mappedChannel)
        {
            Channels = channels;
            MappedChannel = mappedChannel;
        }

        public MappedChannelEditorViewModel() { }

        public IEnumerable<Channel> Channels { get; private set; }

        public MappedChannel MappedChannel { get; private set; }
    }
}