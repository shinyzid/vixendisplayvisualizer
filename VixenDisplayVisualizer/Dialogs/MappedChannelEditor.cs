namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Windows.Forms;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    public partial class MappedChannelEditor : Form
    {
        public MappedChannelEditor(MappedChannelEditorViewModel viewModel)
        {
            InitializeComponent();
            mappedChannelEditorView.DataContext = viewModel;
        }
    }
}
