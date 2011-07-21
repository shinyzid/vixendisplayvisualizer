// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System;
    using System.Windows.Forms;

    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    /// <summary>
    ///   The display visualizer.
    /// </summary>
    public partial class DisplayVisualizer : Form
    {
        /// <summary>
        ///   The _view model.
        /// </summary>
        private readonly VisualizerViewModel _viewModel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisplayVisualizer" /> class.
        /// </summary>
        /// <param name = "viewModel">
        ///   The view model.
        /// </param>
        public DisplayVisualizer(VisualizerViewModel viewModel)
        {
            this.InitializeComponent();
            this.visualizerView.DataContext = viewModel;
            this._viewModel = viewModel;

            var workingArea = Screen.PrimaryScreen.WorkingArea;
            var width = viewModel.DisplayWidth + 50;
            var height = viewModel.DisplayHeight + 65;
            this.Width = Math.Min(workingArea.Width, width == 0 ? 800 : width);
            this.Height = Math.Min(workingArea.Height, height == 0 ? 600 : height);
        }

        /// <summary>
        ///   The update with.
        /// </summary>
        /// <param name = "channelValues">
        ///   The channel values.
        /// </param>
        public void UpdateWith(byte[] channelValues)
        {
            this._viewModel.UpdateWith(channelValues);
        }
    }
}