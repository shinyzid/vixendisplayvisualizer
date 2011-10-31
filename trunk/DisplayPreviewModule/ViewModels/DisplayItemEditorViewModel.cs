namespace Vixen.Modules.DisplayPreviewModule.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Vixen.Modules.DisplayPreviewModule.Model;
    using Vixen.Sys;

    public class DisplayItemEditorViewModel : ViewModelBase
    {
        private DisplayItem _displayItem;
        private ObservableCollection<ChannelNode> _channelNodes;

        public DisplayItemEditorViewModel(DisplayItem displayItem)
        {
            _displayItem = displayItem;
            var rootNodes = VixenSystem.Nodes.GetRootNodes().ToList();
            ChannelNodes = new ObservableCollection<ChannelNode>(rootNodes);
        }

        public ObservableCollection<ChannelNode> ChannelNodes
        {
            get
            {
                return _channelNodes;
            }
            set
            {
                _channelNodes = value;
                this.OnPropertyChanged("ChannelNodes");
            }
        }

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
