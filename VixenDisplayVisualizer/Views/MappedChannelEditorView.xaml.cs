// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Views
{
    using System;
    using System.Windows;

    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;

    /// <summary>
    ///   Interaction logic for MappedChannelEditorView.xaml
    /// </summary>
    public partial class MappedChannelEditorView
    {
        public static readonly DependencyProperty MappedChannelProperty = DependencyProperty.Register(
            "MappedChannel",
            typeof(MappedChannel),
            typeof(MappedChannelEditorView),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(MappedChannelChanged)));

        private static void MappedChannelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // var mappedChannel = (MappedChannel)e.NewValue;
            // var source = (MappedChannedlEditorView)d;
        }

        public MappedChannelEditorView()
        {
            this.InitializeComponent();
        }

        public MappedChannel MappedChannel
        {
            get
            {
                return (MappedChannel)this.GetValue(MappedChannelProperty);
            }

            set
            {
                this.SetValue(MappedChannelProperty, value);
            }
        }
    }
}