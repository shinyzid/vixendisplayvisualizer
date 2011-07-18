// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Windows.Forms;

    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    /// <summary>
    ///   The element editor.
    /// </summary>
    public partial class ElementEditor : Form
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ElementEditor" /> class.
        /// </summary>
        /// <param name = "elementEditorViewModel">
        ///   The element editor view model.
        /// </param>
        public ElementEditor(ElementEditorViewModel elementEditorViewModel)
        {
            this.InitializeComponent();
            this.elementEditor1.DataContext = elementEditorViewModel;
        }
    }
}