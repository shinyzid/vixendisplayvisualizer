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
            if (((this._displayVisualizer != null) && !this._displayVisualizer.Disposing)
                && !this._displayVisualizer.IsDisposed)
            {
                this._displayVisualizer.UpdateWith(channelValues);
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
            this._channels.Clear();
            this._channels.AddRange(executableObject.Channels);
            this._setupData = setupData;
            this._setupNode = setupNode;
            this._startChannel = Convert.ToInt32(this._setupNode.Attributes["from"].Value) - 1;
            this._setupData.GetBytes(this._setupNode, "BackgroundImage", new byte[0]);
        }

        /// <summary>
        /// The shutdown.
        /// </summary>
        public void Shutdown()
        {
            if (this._displayVisualizer != null)
            {
                if (this._displayVisualizer.InvokeRequired)
                {
                    this._displayVisualizer.BeginInvoke(new MethodInvoker(this._displayVisualizer.Dispose));
                }
                else
                {
                    this._displayVisualizer.Dispose();
                }

                this._displayVisualizer = null;
            }

            this._channels.Clear();
            this._setupData = null;
            this._setupNode = null;
        }

        /// <summary>
        /// The startup.
        /// </summary>
        public void Startup()
        {
            if (this._channels.Any())
            {
                var system = (ISystem)Interfaces.Available["ISystem"];
                var constructor =
                    typeof(DisplayVisualizer).GetConstructor(
                        new[] { typeof(XmlNode), typeof(List<Channel>), typeof(int) });
                this._displayVisualizer =
                    (DisplayVisualizer)
                    system.InstantiateForm(
                        constructor, new object[] { this._setupNode, this._channels, this._startChannel });
            }
        }

        /// <summary>
        /// The setup.
        /// </summary>
        public void Setup()
        {
            if (this._channels.Any())
            {
                using (this._setupDialog = new Setup())
                {
                    this._setupDialog.ShowDialog();
                }

                this._setupDialog = null;
            }
            else
            {
                MessageBox.Show(
                    "There are not channels assigned to this plugin.", 
                    this.Name, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Hand);
            }
        }
    }
}