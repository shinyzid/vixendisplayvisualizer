// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Windows.Forms;

    /// <summary>
    /// The display visualizer.
    /// </summary>
    public partial class DisplayVisualizer : Form
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisplayVisualizer" /> class.
        /// </summary>
        public DisplayVisualizer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The update with.
        /// </summary>
        /// <param name="channelValues">
        /// The channel values.
        /// </param>
        public void UpdateWith(byte[] channelValues)
        {
        }
    }
}