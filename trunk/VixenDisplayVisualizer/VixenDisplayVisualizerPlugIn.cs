// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Xml;

    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    using MessageBox = System.Windows.MessageBox;

    /// <summary>
    /// The vixen display visualizer plug in.
    /// </summary>
    public class VixenDisplayVisualizerPlugIn : IEventDrivenOutputPlugIn
    {
        /// <summary>
        ///   The _channels.
        /// </summary>
        private readonly List<Channel> _channels = new List<Channel>();

        /// <summary>
        ///   The _display visualizer.
        /// </summary>
        private DisplayVisualizer _displayVisualizer;

        /// <summary>
        ///   The _setup data.
        /// </summary>
        private SetupData _setupData;

        /// <summary>
        ///   The _setup dialog.
        /// </summary>
        private Setup _setupDialog;

        /// <summary>
        ///   The _setup node.
        /// </summary>
        private XmlNode _setupNode;

        /// <summary>
        ///   The _start channel.
        /// </summary>
        private int _startChannel;

        /// <summary>
        ///   Gets Author.
        /// </summary>
        public string Author
        {
            get
            {
                return "Erik Mathisen - erik@mathisen.us";
            }
        }

        /// <summary>
        ///   Gets Description.
        /// </summary>
        /// <exception cref = "NotImplementedException">
        /// </exception>
        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Gets HardwareMap.
        /// </summary>
        public HardwareMap[] HardwareMap
        {
            get
            {
                return new HardwareMap[0];
            }
        }

        /// <summary>
        ///   Gets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return "Vixen Display Visualizer";
            }
        }

        /// <summary>
        /// The event.
        /// </summary>
        /// <param name="channelValues">
        /// The channel values.
        /// </param>
        public void Event(byte[] channelValues)
        {
            if (((_displayVisualizer != null) && !_displayVisualizer.Disposing) && !_displayVisualizer.IsDisposed)
            {
                _displayVisualizer.UpdateWith(channelValues);
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="executableObject">
        /// The executable object.
        /// </param>
        /// <param name="setupData">
        /// The setup data.
        /// </param>
        /// <param name="setupNode">
        /// The setup node.
        /// </param>
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            _channels.Clear();
            _channels.AddRange(executableObject.Channels);
            _setupData = setupData;
            _setupNode = setupNode;
            _startChannel = Convert.ToInt32(_setupNode.Attributes["from"].Value) - 1;
            _setupData.GetBytes(_setupNode, "BackgroundImage", new byte[0]);
        }

        /// <summary>
        /// The shutdown.
        /// </summary>
        public void Shutdown()
        {
            if (_displayVisualizer != null)
            {
                if (_displayVisualizer.InvokeRequired)
                {
                    _displayVisualizer.BeginInvoke(new MethodInvoker(_displayVisualizer.Dispose));
                }
                else
                {
                    _displayVisualizer.Dispose();
                }

                _displayVisualizer = null;
            }

            _channels.Clear();
            _setupData = null;
            _setupNode = null;
        }

        /// <summary>
        /// The startup.
        /// </summary>
        public void Startup()
        {
            if (_channels.Any())
            {
                var system = (ISystem)Interfaces.Available["ISystem"];
                var constructor =
                    typeof(DisplayVisualizer).GetConstructor(
                        new[] { typeof(XmlNode), typeof(List<Channel>), typeof(int) });
                _displayVisualizer =
                    (DisplayVisualizer)
                    system.InstantiateForm(constructor, new object[] { _setupNode, _channels, _startChannel });
            }
        }

        /// <summary>
        /// The setup.
        /// </summary>
        public void Setup()
        {
            if (_channels.Any())
            {
                var viewModel = new SetupViewModel();
                _channels.ForEach(x => viewModel.Channels.Add(x));
                using (_setupDialog = new Setup(viewModel))
                {
                    _setupDialog.ShowDialog();
                }

                _setupDialog = null;
            }
            else
            {
                MessageBox.Show(
                    "There are not channels assigned to this plugin.", Name, MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}