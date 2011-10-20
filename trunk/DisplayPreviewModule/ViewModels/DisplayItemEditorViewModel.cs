namespace Vixen.Modules.DisplayPreviewModule.ViewModels
{
    using System.Collections.Generic;
    using Vixen.Modules.DisplayPreviewModule.Model;
    using Vixen.Sys;

    public class DisplayItemEditorViewModel : ViewModelBase
    {
        private DisplayItem _displayItem;

        public DisplayItemEditorViewModel(IEnumerable<Channel> channels, DisplayItem displayItem)
        {
            Channels = channels;
            _displayItem = displayItem;
        }

        public IEnumerable<Channel> Channels { get; private set; }

        public DisplayItem DisplayItem
        {
            get
            {
                return _displayItem;
            }

            set
            {
                _displayItem = value;
                OnPropertyChanged("displayItem");
            }
        }
    }
}
