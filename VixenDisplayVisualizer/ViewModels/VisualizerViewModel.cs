// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Imaging;

    /// <summary>
    ///   The visualizer view model.
    /// </summary>
    public class VisualizerViewModel : ViewModelBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "VisualizerViewModel" /> class.
        /// </summary>
        /// <param name = "channels">
        ///   The channels.
        /// </param>
        /// <param name = "displayElements">
        ///   The display elements.
        /// </param>
        public VisualizerViewModel(List<Channel> channels, List<DisplayElement> displayElements, BitmapSource backgroundImage, int displayWidth, int displayHeight)
        {
            this.Channels = channels;
            this.DisplayElements = displayElements;
            BackgroundImage = backgroundImage;
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
        }

        public int DisplayHeight { get; set; }

        public int DisplayWidth { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "VisualizerViewModel" /> class.
        /// </summary>
        public VisualizerViewModel()
        {
            this.Channels = new List<Channel>();
            this.DisplayElements = new List<DisplayElement>();
            BackgroundImage = null;
        }

        private BitmapSource _backgroundImage;
        public BitmapSource BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }
            set
            {
                _backgroundImage = value;
                OnPropertyChanged("BackgroundImage");
            }
        }

        /// <summary>
        ///   Gets or sets Channels.
        /// </summary>
        public List<Channel> Channels { get; set; }

        /// <summary>
        ///   Gets or sets DisplayElements.
        /// </summary>
        public List<DisplayElement> DisplayElements { get; set; }

        /// <summary>
        ///   The update with.
        /// </summary>
        /// <param name = "channelValues">
        ///   The channel values.
        /// </param>
        public void UpdateWith(byte[] channelValues)
        {
            if (channelValues == null)
            {
                return;
            }

            for (var index = 0; index < channelValues.Length; index++)
            {
                var channel = this.Channels[index];
                var color = channelValues[index];
                var mappedChannels = (from displayElement in this.DisplayElements
                                      where displayElement != null
                                      from mappedChannel in displayElement.PixelMappings
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