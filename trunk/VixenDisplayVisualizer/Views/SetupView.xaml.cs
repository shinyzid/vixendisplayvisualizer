// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Views
{
    using System.Windows;

    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SetupView" /> class.
        /// </summary>
        public SetupView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The add button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            using (var editor = new ElementEditor())
            {
                editor.ShowDialog();
            }
        }
    }
}