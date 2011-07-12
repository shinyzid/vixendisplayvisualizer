namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class VisualizerViewModel : ViewModelBase
    {
        public VisualizerViewModel(List<Channel> channels, List<DisplayElement> displayElements)
        {
            Channels = channels;
            DisplayElements = displayElements;
        }

        public VisualizerViewModel()
        {
            Channels = new List<Channel>();
            DisplayElements = new List<DisplayElement>();
        }

        public List<Channel> Channels { get; set; }

        public List<DisplayElement> DisplayElements { get; set; }

        public void UpdateWith(byte[] channelValues)
        {
            for (var index = 0; index < channelValues.Length; index++)
            {
                var channel = Channels[index];
                var color = channelValues[index];
                var mappedChannels = (from displayElement in DisplayElements
                                      from mappedChannel in displayElement.MappedChannels
                                      where mappedChannel.Contains(channel)
                                      select mappedChannel).ToList();
                foreach (var mappedChannel in mappedChannels)
                {
                    mappedChannel.SetColor(channel, color);
                }
            }
        }
    }
}
