// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;
    using Vixen.PlugIns.VixenDisplayVisualizer.Pixels;

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
        ///   The _display height.
        /// </summary>
        private int _displayHeight;

        /// <summary>
        ///   The _display width.
        /// </summary>
        private int _displayWidth;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SetupViewModel" /> class.
        /// </summary>
        public SetupViewModel(int displayWidth, int displayHeight, ImageSource backgroundImage)
        {
            this.AddElementCommand = new RelayCommand(x => this.AddElement());
            this.EditElementCommand = new RelayCommand(
                x => this.EditDisplayElement(), x => this.CanEditDisplayElement());
            this.DeleteElementCommand = new RelayCommand(
                x => this.DeleteDisplayElement(), x => this.CanDeleteDisplayElement());
            this.SetBackgroundCommand = new RelayCommand(x => SetBackground());
            this.DisplayElements = new ObservableCollection<DisplayElement>();
            this.Channels = new ObservableCollection<Channel>();
            this.DisplayWidth = displayWidth == 0 ? 800 : displayWidth;
            this.DisplayHeight = displayHeight == 0 ? 600 : displayHeight;
            BackgroundImage = backgroundImage;
        }

        private void SetBackground()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.FileName = "Image"; // Default file name
            openFileDialog.DefaultExt = ".bmp|.jpg|.png"; // Default file extension
            openFileDialog.Filter = "Imageds (.bmp)|*.bmp|*.png|*.jpg"; // Filter files by extension

            // Show open file dialog box
            var result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = openFileDialog.FileName;
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filename, UriKind.Absolute);
                image.EndInit();
                BackgroundImage = image;
            }
        }

        public ICommand SetBackgroundCommand { get; private set; }

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
                return this._currentDisplayElement;
            }

            set
            {
                this._currentDisplayElement = value;
                this.OnPropertyChanged("CurrentDisplayElement");
            }
        }

        private ImageSource _backgroundImage;
        public ImageSource BackgroundImage
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

        /// <summary>
        ///   Gets DeleteElementCommand.
        /// </summary>
        public ICommand DeleteElementCommand { get; private set; }

        /// <summary>
        ///   Gets or sets DisplayElements.
        /// </summary>
        public ObservableCollection<DisplayElement> DisplayElements { get; set; }

        /// <summary>
        ///   Gets or sets DisplayHeight.
        /// </summary>
        public int DisplayHeight
        {
            get
            {
                return this._displayHeight;
            }

            set
            {
                this._displayHeight = value;
                this.OnPropertyChanged("DisplayHeight");
            }
        }

        /// <summary>
        ///   Gets or sets DisplayWidth.
        /// </summary>
        public int DisplayWidth
        {
            get
            {
                return this._displayWidth;
            }

            set
            {
                this._displayWidth = value;
                this.OnPropertyChanged("DisplayWidth");
            }
        }

        /// <summary>
        ///   Gets EditElementCommand.
        /// </summary>
        public ICommand EditElementCommand { get; private set; }

        /// <summary>
        ///   The add element.
        /// </summary>
        private void AddElement()
        {
            var displayElement = new DisplayElement(10, 10, 100, 0, 0, 100, new List<PixelMapping>())
                { Name = "My New Element" };
            var viewModel = new ElementEditorViewModel(this.Channels, displayElement);
            using (var editor = new ElementEditor(viewModel))
            {
                editor.ShowDialog();
                this.DisplayElements.Add(displayElement);
                this.CurrentDisplayElement = displayElement;
            }
        }

        /// <summary>
        ///   The can delete display element.
        /// </summary>
        /// <returns>
        ///   The can delete display element.
        /// </returns>
        private bool CanDeleteDisplayElement()
        {
            return this.CurrentDisplayElement != null;
        }

        /// <summary>
        ///   The can edit display element.
        /// </summary>
        /// <returns>
        ///   The can edit display element.
        /// </returns>
        private bool CanEditDisplayElement()
        {
            return this.CurrentDisplayElement != null;
        }

        /// <summary>
        ///   The delete display element.
        /// </summary>
        private void DeleteDisplayElement()
        {
            var displayElement = this.CurrentDisplayElement;
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
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                this.DisplayElements.Remove(displayElement);
                this.CurrentDisplayElement = null;
            }
        }

        /// <summary>
        ///   The edit display element.
        /// </summary>
        private void EditDisplayElement()
        {
            var displayElement = this.CurrentDisplayElement;
            if (displayElement == null)
            {
                return;
            }

            var viewModel = new ElementEditorViewModel(this.Channels, displayElement);
            using (var editor = new ElementEditor(viewModel))
            {
                editor.ShowDialog();
            }
        }
    }
}