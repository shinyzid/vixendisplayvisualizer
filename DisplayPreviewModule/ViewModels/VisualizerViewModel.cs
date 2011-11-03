namespace Vixen.Modules.DisplayPreviewModule.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Vixen.Modules.DisplayPreviewModule.Model;
    using Vixen.Sys;

    public class VisualizerViewModel : ViewModelBase
    {
        private string _backgroundImage;

        public VisualizerViewModel(DisplayPreviewModuleDataModel displayPreviewModuleDataModel)
        {
            DisplayElements = displayPreviewModuleDataModel.DisplayItems;
            BackgroundImage = displayPreviewModuleDataModel.BackgroundImage;
            DisplayWidth = displayPreviewModuleDataModel.DisplayWidth;
            DisplayHeight = displayPreviewModuleDataModel.DisplayHeight;
        }

        public string BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }

            set
            {
                _backgroundImage = value;
                OnPropertyChanged("BackgroundImage");
            }
        }

        public ObservableCollection<DisplayItem> DisplayElements { get; set; }

        public int DisplayHeight { get; set; }

        public int DisplayWidth { get; set; }

        public void UpdateExecutionStateValues(ExecutionStateValues stateValues)
        {
            foreach (var executionStateValue in stateValues)
            {
                Console.WriteLine(executionStateValue.Key.Id);
                Console.WriteLine(executionStateValue.Value.GetParameterValue(0));
            }
        }
    }
}
