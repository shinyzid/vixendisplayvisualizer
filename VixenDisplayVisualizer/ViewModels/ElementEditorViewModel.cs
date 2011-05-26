// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    /// <summary>
    /// The element editor view model.
    /// </summary>
    public class ElementEditorViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementEditorViewModel"/> class.
        /// </summary>
        /// <param name="channels">
        /// The channels.
        /// </param>
        /// <param name="mappedChannels">
        /// The mapped channels.
        /// </param>
        public ElementEditorViewModel(IEnumerable<Channel> channels, IEnumerable<MappedChannel> mappedChannels)
        {
            this.Channels = channels;
            this.MappedChannels = mappedChannels;
        }

        public ElementEditorViewModel()
        {
            
        }

        /// <summary>
        /// Gets Channels.
        /// </summary>
        public IEnumerable<Channel> Channels { get; private set; }

        /// <summary>
        /// Gets MappedChannels.
        /// </summary>
        public IEnumerable<MappedChannel> MappedChannels { get; private set; }
    }
}