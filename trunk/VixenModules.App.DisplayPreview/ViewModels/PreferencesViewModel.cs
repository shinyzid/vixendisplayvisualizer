namespace VixenModules.App.DisplayPreview.ViewModels
{
    using VixenModules.App.DisplayPreview.Model;

    public class PreferencesViewModel : ViewModelBase
    {
        private readonly DisplayPreviewModuleDataModel _dataModel;

        public PreferencesViewModel(DisplayPreviewModuleDataModel displayPreviewModuleDataModel)
        {
            _dataModel = displayPreviewModuleDataModel;
        }

        public Preferences Preferences
        {
            get
            {
                return _dataModel.Preferences;
            }

            set
            {
                _dataModel.Preferences = value;
                OnPropertyChanged("Preferences");
            }
        }
    }
}