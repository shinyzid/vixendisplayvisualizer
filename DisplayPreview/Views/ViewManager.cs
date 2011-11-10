namespace VixenModules.App.DisplayPreview.Views
{
    using System;
    using System.Windows.Threading;
    using Vixen.Sys;
    using VixenModules.App.DisplayPreview.Model;
    using VixenModules.App.DisplayPreview.ViewModels;

    public static class ViewManager
    {
        private static VisualizerView _view;

        private static VisualizerViewModel _visualizerViewModel;

        public static bool IsVisualizerRunning { get; private set; }

        public static void DisplaySetupView(DisplayPreviewModuleDataModel dataModel)
        {
            var setupViewModel = new SetupViewModel(dataModel);
            var setupView = new SetupView { DataContext = setupViewModel };
            setupView.ShowDialog();
        }

        public static void EnsureVisualizerIsClosed()
        {
            if (_view != null)
            {
                _view.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => _view.Close()));
            }
        }

        public static void StartVisualizer(DisplayPreviewModuleDataModel dataModel)
        {
            if (IsVisualizerRunning)
            {
                _view.Focus();
            }
            else
            {
                _visualizerViewModel = new VisualizerViewModel(dataModel);
                _view = new VisualizerView { DataContext = _visualizerViewModel };
                _view.Closed += VisualizerViewClosed;
                _view.Show();
                IsVisualizerRunning = true;
            }
        }

        public static void UpdatePreviewExecutionStateValues(ExecutionStateValues stateValues)
        {
            if (_visualizerViewModel != null)
            {
                _visualizerViewModel.UpdateExecutionStateValues(stateValues);
            }
        }

        private static void VisualizerViewClosed(object sender, EventArgs e)
        {
            if (_view != null)
            {
                _view.Closed -= VisualizerViewClosed;
                _view = null;
                _visualizerViewModel = null;
            }

            IsVisualizerRunning = false;
        }
    }
}