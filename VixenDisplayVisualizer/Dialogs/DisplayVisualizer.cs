namespace Vixen.PlugIns.VixenDisplayVisualizer.Dialogs
{
    using System.Windows.Forms;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    public partial class DisplayVisualizer : Form
    {
        private readonly VisualizerViewModel _viewModel;

        public DisplayVisualizer(VisualizerViewModel viewModel)
        {
            InitializeComponent();
            visualizerView.DataContext = viewModel;
            _viewModel = viewModel;
        }

        public void UpdateWith(byte[] channelValues)
        {
            _viewModel.UpdateWith(channelValues);
        }
    }
}
