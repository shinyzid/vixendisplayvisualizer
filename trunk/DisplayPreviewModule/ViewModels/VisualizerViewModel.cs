namespace Vixen.Modules.DisplayPreviewModule.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using Vixen.Modules.DisplayPreviewModule.Model;
    using Vixen.Sys;

    public class VisualizerViewModel : ViewModelBase
    {
        private BitmapSource _backgroundImage;

        public VisualizerViewModel(DisplayPreviewModuleDataModel displayPreviewModuleDataModel)
        {
            Channels = displayPreviewModuleDataModel.Channels;
            DisplayElements = displayPreviewModuleDataModel.DisplayElements;
            BackgroundImage = displayPreviewModuleDataModel.BackgroundImage;
            DisplayWidth = displayPreviewModuleDataModel.DisplayWidth;
            DisplayHeight = displayPreviewModuleDataModel.DisplayHeight;
        }

        public BitmapSource BackgroundImage
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

        public List<Channel> Channels { get; set; }

        public List<DisplayItem> DisplayElements { get; set; }

        public int DisplayHeight { get; set; }

        public int DisplayWidth { get; set; }

        public void UpdateExecutionStateValues(ExecutionStateValues stateValues)
        {
            // TODO
        }
    }
}
