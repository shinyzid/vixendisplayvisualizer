namespace Vixen.Modules.DisplayPreviewModule.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32;
    using Vixen.Modules.DisplayPreviewModule.Model;
    using Vixen.Modules.DisplayPreviewModule.Views;
    using Vixen.Modules.DisplayPreviewModule.WPF;
    using Vixen.Sys;

    public class SetupViewModel : ViewModelBase
    {
        private BitmapSource _backgroundImage;

        private DisplayItem _currentDisplayElement;

        private int _displayHeight;

        private int _displayWidth;

        private double _opacity;

        public SetupViewModel(DisplayPreviewModuleDataModel dataModel)
        {
            AddElementCommand = new RelayCommand(x => AddElement());
            EditElementCommand = new RelayCommand(x => EditDisplayElement(), x => CanEditDisplayElement());
            DeleteElementCommand = new RelayCommand(x => DeleteDisplayElement(), x => CanDeleteDisplayElement());
            SetBackgroundCommand = new RelayCommand(x => SetBackground());
            MoveUpCommand = new RelayCommand(x => MoveUp(), x => CanMoveUp());
            MoveDownCommand = new RelayCommand(x => MoveDown(), x => CanMoveDown());
            DisplayElements = new ObservableCollection<DisplayItem>();
            Channels = new ObservableCollection<Channel>();
            DisplayWidth = dataModel.DisplayWidth == 0 ? 800 : dataModel.DisplayWidth;
            DisplayHeight = dataModel.DisplayHeight == 0 ? 600 : dataModel.DisplayHeight;
            BackgroundImage = dataModel.BackgroundImage;
            Opacity = dataModel.Opactity;
        }

        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                this.OnPropertyChanged("Opacity");
            }
        }

        public ICommand AddElementCommand { get; private set; }

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

        public ObservableCollection<Channel> Channels { get; private set; }

        public DisplayItem CurrentDisplayElement
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

        public ICommand DeleteElementCommand { get; private set; }

        public ObservableCollection<DisplayItem> DisplayElements { get; set; }

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

        public ICommand EditElementCommand { get; private set; }

        public ICommand MoveDownCommand { get; private set; }
        
        public ICommand MoveUpCommand { get; private set; }
        
        public ICommand SetBackgroundCommand { get; private set; }

        /// <summary>
        ///   The add element.
        /// </summary>
        private void AddElement()
        {
            var displayElement = new DisplayItem(100, 100, 0, 0, new List<ChannelLocation>(), true, Channels) { Name = "My New Element" };
            var viewModel = new DisplayItemEditorViewModel(Channels, displayElement);
            var editor = new DisplayItemEditorView();
            editor.DataContext = viewModel;
            editor.ShowDialog();
            DisplayElements.Add(displayElement);
            CurrentDisplayElement = displayElement;
        }

        private bool CanDeleteDisplayElement()
        {
            return CurrentDisplayElement != null;
        }

        private bool CanEditDisplayElement()
        {
            return CurrentDisplayElement != null;
        }

        private bool CanMoveDown()
        {
            var currentDisplayElement = CurrentDisplayElement;
            return currentDisplayElement != null && DisplayElements.IndexOf(currentDisplayElement) != DisplayElements.Count - 1;
        }

        private bool CanMoveUp()
        {
            var currentDisplayElement = CurrentDisplayElement;
            return currentDisplayElement != null && DisplayElements.IndexOf(currentDisplayElement) != 0;
        }

        private void DeleteDisplayElement()
        {
            var displayElement = CurrentDisplayElement;
            if (displayElement == null)
            {
                return;
            }

            if (
                MessageBox.Show(
                                string.Format(
                                              "Are you sure you want to delete the selected display element named '{0}' ?", 
                                              displayElement.Name), 
                                "Confirm delete", 
                                MessageBoxButton.YesNo, 
                                MessageBoxImage.Question,
                                MessageBoxResult.No)
                == MessageBoxResult.Yes)
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

            var viewModel = new DisplayItemEditorViewModel(Channels, displayElement);
            var editor = new DisplayItemEditorView();
            editor.DataContext = viewModel;
            editor.ShowDialog();
        }

        private void MoveDown()
        {
            var currentDisplayElement = CurrentDisplayElement;
            var index = DisplayElements.IndexOf(currentDisplayElement);
            DisplayElements.Move(index, index + 1);
        }

        private void MoveUp()
        {
            var currentDisplayElement = CurrentDisplayElement;
            var index = DisplayElements.IndexOf(currentDisplayElement);
            DisplayElements.Move(index, index - 1);
        }

        private void SetBackground()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".bmp|.jpg|.png"; // Default file extension
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; // Filter files by extension

            // Show open file dialog box
            var result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                var filename = openFileDialog.FileName;
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filename, UriKind.Absolute);
                image.EndInit();
                BackgroundImage = image;
                Opacity = 1;
            }
        }
    }
}
