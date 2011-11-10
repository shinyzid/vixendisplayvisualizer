namespace VixenModules.App.DisplayPreview.ViewModels
{
    using System.Collections.ObjectModel;
    using Vixen.Sys;
    using VixenModules.App.DisplayPreview.Model;
    using VixenModules.Property.RGB;

    public class VisualizerViewModel : ViewModelBase
    {
        private string _backgroundImage;
        private DisplayPreviewModuleDataModel _dataModel;

        public VisualizerViewModel(DisplayPreviewModuleDataModel displayPreviewModuleDataModel)
        {
            _dataModel = displayPreviewModuleDataModel;
            DisplayItems = displayPreviewModuleDataModel.DisplayItems;
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

        public double Opacity
        {
            get
            {
                return _dataModel.Opacity;
            }

            set
            {
                _dataModel.Opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public ObservableCollection<DisplayItem> DisplayItems { get; set; }

        public int DisplayHeight { get; set; }

        public int DisplayWidth { get; set; }

        public void UpdateExecutionStateValues(ExecutionStateValues stateValues)
        {
            var colorsByChannel = RGBModule.MapChannelCommandsToColors(stateValues).ToMediaColor();
            foreach (var displayItem in DisplayItems)
            {
                displayItem.UpdateChannelColors(colorsByChannel);
            }
        }
    }
}
