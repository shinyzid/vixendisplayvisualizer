namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    /// <summary>
    ///   The setup view model.
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
            EditElementCommand = new RelayCommand(x => EditDisplayElement(), x => CanEditDisplayElement());
            DeleteElementCommand = new RelayCommand(x => DeleteDisplayElement(), x => CanDeleteDisplayElement());
            DisplayElements = new ObservableCollection<DisplayElement>();
            Channels = new ObservableCollection<Channel>();
            DisplayWidth = 800;
            DisplayHeight = 600;
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

        private int _displayWidth;
        public int DisplayWidth
        {
            get
            {
                return _displayWidth;
            }
            set
            {
                _displayWidth = value;
                OnPropertyChanged("DisplayWidth");
            }
        }

        private int _displayHeight;
        public int DisplayHeight
        {
            get
            {
                return _displayHeight;
            }
            set
            {
                _displayHeight = value;
                OnPropertyChanged("DisplayHeight");
            }
        }

        public ICommand DeleteElementCommand { get; private set; }

        /// <summary>
        ///   Gets or sets DisplayElements.
        /// </summary>
        public ObservableCollection<DisplayElement> DisplayElements { get; set; }

        public ICommand EditElementCommand { get; private set; }

        /// <summary>
        ///   The add element.
        /// </summary>
        private void AddElement()
        {
            var displayElement = new DisplayElement(10, 10, 100, 0, 0, 100, new List<MappedChannel>());
            displayElement.Name = "My New Element";
            var viewModel = new ElementEditorViewModel(Channels, displayElement);
            using (var editor = new ElementEditor(viewModel))
            {
                editor.ShowDialog();
                DisplayElements.Add(displayElement);
                CurrentDisplayElement = displayElement;
            }
        }

        private bool CanDeleteDisplayElement()
        {
            return CurrentDisplayElement != null;
        }

        private bool CanEditDisplayElement()
        {
            return CurrentDisplayElement != null;
        }

        private void DeleteDisplayElement()
        {
            var displayElement = CurrentDisplayElement;
            if (displayElement == null)
            {
                return;
            }

            if (MessageBox.Show(
                                string.Format("Are you sure you want to delete the selected display element named '{0}' ?", displayElement.Name), 
                                "Confirm delete", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question, 
                                MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
            {
                DisplayElements.Remove(displayElement);
                CurrentDisplayElement = null;
            }
        }

        private void EditDisplayElement()
        {
            var displayElement = CurrentDisplayElement;
            if (displayElement == null)
            {
                return;
            }

            var viewModel = new ElementEditorViewModel(Channels, displayElement);
            using (var editor = new ElementEditor(viewModel))
            {
                editor.ShowDialog();
            }
        }
    }
}
