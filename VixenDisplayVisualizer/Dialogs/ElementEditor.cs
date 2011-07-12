namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Windows.Forms;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    public partial class ElementEditor : Form
    {
        public ElementEditor(ElementEditorViewModel elementEditorViewModel)
        {
            InitializeComponent();
            elementEditor1.DataContext = elementEditorViewModel;
        }
    }
}
