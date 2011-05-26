// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Windows.Forms;

    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    /// <summary>
    /// The setup.
    /// </summary>
    public partial class Setup : Form
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "Setup" /> class.
        /// </summary>
        /// <param name="viewModel"></param>
        public Setup(SetupViewModel viewModel)
        {
            this.InitializeComponent();
            this.setupView1.DataContext = viewModel;
        }
    }
}