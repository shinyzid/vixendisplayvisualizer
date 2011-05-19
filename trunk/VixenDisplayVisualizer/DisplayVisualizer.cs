// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayVisualizer.cs" company="Erik Mathisen">
//   2011
// </copyright>
// <summary>
//   The display visualizer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Windows.Forms;

    /// <summary>
    /// The display visualizer.
    /// </summary>
    public partial class DisplayVisualizer : Form
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayVisualizer"/> class.
        /// </summary>
        public DisplayVisualizer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The update with.
        /// </summary>
        /// <param name="channelValues">
        /// The channel values.
        /// </param>
        public void UpdateWith(byte[] channelValues)
        {
        }

        #endregion
    }
}