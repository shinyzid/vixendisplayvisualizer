using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    public partial class MappedChannelEditor : Form
    {
        public MappedChannelEditor(MappedChannel mappedChannel)
        {
            InitializeComponent();
            this.mappedChannelEditorView1.MappedChannel = mappedChannel;
        }
    }
}
