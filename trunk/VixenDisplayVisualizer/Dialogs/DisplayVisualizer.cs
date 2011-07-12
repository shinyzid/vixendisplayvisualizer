// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public partial class DisplayVisualizer : Form
    {
        public DisplayVisualizer(List<Channel> channels, List<MappedChannel> mappedChannels)
        {
            this.InitializeComponent();
        }

        public void UpdateWith(byte[] channelValues)
        {
        }
    }
}