// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The setup view model.
    /// </summary>
    public class SetupViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupViewModel"/> class.
        /// </summary>
        public SetupViewModel()
        {
            this.DisplayElements = new ObservableCollection<DisplayElement>();
        }

        /// <summary>
        /// Gets or sets DisplayElements.
        /// </summary>
        public ObservableCollection<DisplayElement> DisplayElements { get; set; }
    }
}