// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;

    /// <summary>
    ///   Interaction logic for MappedChannelView.xaml
    /// </summary>
    public partial class MappedChannelView
    {
        public ICommand AddMappedChannelCommand { get; private set; }

        public MappedChannelView()
        {
            AddMappedChannelCommand = new RelayCommand(x => AddMappedChannel());
            this.InitializeComponent();           
        }

        private void AddMappedChannel()
        {
            var mappedChannel = new MappedChannel(null);
            using (var mapped = new MappedChannelEditor(mappedChannel))
            {
                mapped.ShowDialog();
            }
        }
    }
}