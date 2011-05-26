// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    /// <summary>
    /// The setup view model.
    /// </summary>
    public class SetupViewModel : ViewModelBase
    {
        /// <summary>
        ///   The _current display element.
        /// </summary>
        private DisplayElement _currentDisplayElement;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SetupViewModel" /> class.
        /// </summary>
        public SetupViewModel()
        {
            AddElementCommand = new RelayCommand(x => AddElement());
            DisplayElements = new ObservableCollection<DisplayElement>();
            Channels = new ObservableCollection<Channel>();
        }

        /// <summary>
        ///   Gets AddElementCommand.
        /// </summary>
        public ICommand AddElementCommand { get; private set; }

        /// <summary>
        ///   Gets Channels.
        /// </summary>
        public ObservableCollection<Channel> Channels { get; private set; }

        /// <summary>
        ///   Gets or sets CurrentDisplayElement.
        /// </summary>
        public DisplayElement CurrentDisplayElement
        {
            get
            {
                return _currentDisplayElement;
            }

            set
            {
                _currentDisplayElement = value;
                OnPropertyChanged("CurrentDisplayElement");
            }
        }

        /// <summary>
        ///   Gets or sets DisplayElements.
        /// </summary>
        public ObservableCollection<DisplayElement> DisplayElements { get; set; }

        /// <summary>
        /// The add element.
        /// </summary>
        private void AddElement()
        {
            using (var editor = new ElementEditor(new ElementEditorViewModel(Channels, new List<MappedChannel>())))
            {
                editor.ShowDialog();
            }
        }
    }
}