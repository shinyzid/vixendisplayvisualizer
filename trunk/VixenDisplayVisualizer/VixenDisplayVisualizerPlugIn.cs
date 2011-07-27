// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml;

    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;
    using Vixen.PlugIns.VixenDisplayVisualizer.Pixels;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;

    using MessageBox = System.Windows.MessageBox;

    /// <summary>
    ///   The vixen display visualizer plug in.
    /// </summary>
    public class VixenDisplayVisualizerPlugIn : IEventDrivenOutputPlugIn
    {
        /// <summary>
        ///   The _channels.
        /// </summary>
        private readonly List<Channel> _channels = new List<Channel>();

        /// <summary>
        ///   The _elements.
        /// </summary>
        private readonly List<DisplayElement> _elements = new List<DisplayElement>();

        /// <summary>
        ///   The _display visualizer.
        /// </summary>
        private DisplayVisualizer _displayVisualizer;

        /// <summary>
        ///   The _plugin channels from.
        /// </summary>
        private int _pluginChannelsFrom;

        /// <summary>
        ///   The _plugin channels to.
        /// </summary>
        private int _pluginChannelsTo;

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

        private int _displayHeight;
        private int _displayWidth;
        private BitmapSource _background;

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
        public string Description
        {
            get
            {
                return
                    "Vixen Display Visualizer Plugin - Allows for the virtual creation of a display, and playing a sequence on it.";
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
        ///   The event.
        /// </summary>
        /// <param name = "channelValues">
        ///   The channel values.
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
        ///   The initialize.
        /// </summary>
        /// <param name = "executableObject">
        ///   The executable object.
        /// </param>
        /// <param name = "setupData">
        ///   The setup data.
        /// </param>
        /// <param name = "setupNode">
        ///   The setup node.
        /// </param>
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this._channels.Clear();
            this._channels.AddRange(executableObject.Channels);
            this._setupData = setupData;
            this._setupNode = setupNode;
            this.LoadDataFromSetupNode();            
        }

        /// <summary>
        ///   The shutdown.
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

#if VIXEN_2_1
        public List<Form> Startup()
#else
        public void Startup()
#endif
        {
            if (this._channels.Any())
            {
                this.LoadDataFromSetupNode();

                var viewModel = new VisualizerViewModel(this._channels, this._elements, _background, _displayWidth, _displayHeight);
                this._displayVisualizer = new DisplayVisualizer(viewModel);
                this._displayVisualizer.Show();
            }

#if VIXEN_2_1
            return new List<Form>(1) {_displayVisualizer};
#endif
        }

        /// <summary>
        ///   The setup.
        /// </summary>
        public void Setup()
        {
            if (this._channels.Any())
            {
                this.LoadDataFromSetupNode();

                var viewModel = new SetupViewModel(_displayWidth, _displayHeight, _background);
                this._channels.ForEach(x => viewModel.Channels.Add(x));
                this._elements.ForEach(x => viewModel.DisplayElements.Add(x));
                bool saveData = false;
                using (this._setupDialog = new Setup(viewModel))
                {
                    this._setupDialog.ShowDialog();

                    var result = MessageBox.Show(
                                                 "Do you want to save the changes made?",
                                                 "Save Changes",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Question,
                                                 MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        saveData = true;
                        this._elements.Clear();
                        this._elements.AddRange(viewModel.DisplayElements);
                        this._displayWidth = viewModel.DisplayWidth;
                        this._displayHeight = viewModel.DisplayHeight;
                        _background = viewModel.BackgroundImage;
                    }
                }

                if (saveData)
                {
                    while (this._setupNode.ChildNodes.Count > 0)
                    {
                        this._setupNode.RemoveChild(this._setupNode.ChildNodes[0]);
                    }

                    var imageBytes = _background.ToByteArray();
                    if (imageBytes.Length > 0)
                    {
                        _setupData.SetBytes(_setupNode, "BackgroundImage", imageBytes);
                    }

                    var heightAttribute = _setupNode.Attributes.GetNamedItem("displayHeight");
                    var widthAttribute = _setupNode.Attributes.GetNamedItem("displayWidth");

                    if (heightAttribute == null)
                    {
                        _setupNode.AppendAttribute("displayHeight", _displayHeight.ToString());
                    }
                    else
                    {
                        heightAttribute.Value = _displayHeight.ToString();
                    }

                    if (widthAttribute == null)
                    {
                        _setupNode.AppendAttribute("displayWidth", _displayWidth.ToString());
                    }
                    else
                    {
                        widthAttribute.Value = _displayWidth.ToString();
                    }

                    foreach (var element in this._elements)
                    {
                        var node = this._setupNode.OwnerDocument.CreateElement("DisplayElement");
                        node.AppendAttribute("Rows", element.Rows.ToString());
                        node.AppendAttribute("Columns", element.Columns.ToString());
                        node.AppendAttribute("Height", element.Height.ToString());
                        node.AppendAttribute("Width", element.Width.ToString());
                        node.AppendAttribute("LeftOffset", element.LeftOffset.ToString());
                        node.AppendAttribute("TopOffset", element.TopOffset.ToString());
                        node.AppendAttribute("Name", element.Name);
                        node.AppendAttribute("IsUnlocked", element.IsUnlocked.ToString());
                        foreach (var mappedChannel in element.PixelMappings)
                        {
                            var mappedNode = node.OwnerDocument.CreateElement("PixelMapping");

                            var pixel = mappedChannel.Pixel;
                            if (pixel != null)
                            {
                                var channelNode = mappedNode.OwnerDocument.CreateElement("Channel");
                                mappedNode.AppendChild(channelNode);
                                if (pixel is EmptyPixel)
                                {
                                    channelNode.AppendAttribute("Type", "Empty");
                                }
                                else if (pixel is SingleColorPixel)
                                {
                                    channelNode.AppendAttribute("Type", "Single");
                                    var singleColorChannel = (SingleColorPixel)pixel;
                                    var vixenChannel = singleColorChannel.Channel;
                                    channelNode.AppendAttribute(
                                                                "ChannelId",
                                                                vixenChannel == null ? string.Empty : vixenChannel.Name);
                                    channelNode.AppendAttribute("Color", singleColorChannel.DisplayColor.ToString());
                                }
                                else
                                {
                                    var rgb = pixel as RedGreenBluePixel;

                                    var redChannel = rgb.RedChannel;
                                    channelNode.AppendAttribute(
                                                                "RedChannel",
                                                                redChannel == null ? string.Empty : redChannel.Name);
                                    var greenChannel = rgb.GreenChannel;
                                    channelNode.AppendAttribute(
                                                                "GreenChannel",
                                                                greenChannel == null ? string.Empty : greenChannel.Name);
                                    var blueChannel = rgb.BlueChannel;
                                    channelNode.AppendAttribute(
                                                                "BlueChannel",
                                                                blueChannel == null ? string.Empty : blueChannel.Name);

                                    var rgbw = pixel as RedGreenBlueWhitePixel;
                                    var type = "RGB";
                                    if (rgbw != null)
                                    {
                                        var whiteChannel = rgbw.WhiteChannel;
                                        channelNode.AppendAttribute(
                                                                    "WhiteChannel",
                                                                    whiteChannel == null ? string.Empty : whiteChannel.Name);
                                        type += "W";
                                    }

                                    channelNode.AppendAttribute("Type", type);
                                }
                            }

                            node.AppendChild(mappedNode);
                        }

                        this._setupNode.AppendChild(node);
                    }
                }

                this.LoadDataFromSetupNode();
                this._setupDialog = null;
            }
            else
            {
                MessageBox.Show(
                    "There are no channels assigned to this plugin.", 
                    this.Name, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Hand);
            }
        }

        /// <summary>
        ///   The load data from setup node.
        /// </summary>
        private void LoadDataFromSetupNode()
        {
            // get the attribute collection and the from/to attributes if available
            var setupNodeAttributes = this._setupNode.Attributes;
            var fromAttribute = setupNodeAttributes.GetNamedItem("from");
            var toAttribute = setupNodeAttributes.GetNamedItem("to");
            var imageData = _setupData.GetBytes(_setupNode, "BackgroundImage", new byte[0]);

            if (imageData.Length > 0)
            {
                var decoder = new JpegBitmapDecoder(new MemoryStream(imageData), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                _background = decoder.Frames[0];
            }
            else
            {
                _background = null;
            }

            // if we got both attributes, try and parse them
            if (fromAttribute != null && toAttribute != null)
            {
                // try to parse both attributes
                this._pluginChannelsFrom = fromAttribute.Value.TryParseInt32(0);
                this._pluginChannelsTo = toAttribute.Value.TryParseInt32(0);

                // if either is zero, make both zero to indicate not setup
                if (this._pluginChannelsFrom == 0 || this._pluginChannelsTo == 0)
                {
                    this._pluginChannelsFrom = 0;
                    this._pluginChannelsTo = 0;
                }
            }

            var heightAttribute = setupNodeAttributes.GetNamedItem("displayHeight");
            var widthAttribute = setupNodeAttributes.GetNamedItem("displayWidth");

            if (widthAttribute != null && heightAttribute != null)
            {
                _displayHeight = heightAttribute.Value.TryParseInt32(0);
                _displayWidth = widthAttribute.Value.TryParseInt32(0);
            }

            this._elements.Clear();
            foreach (XmlNode node in this._setupNode.ChildNodes)
            {
                var attributes = node.Attributes;
                if (attributes == null || attributes.Count == 0)
                {
                    continue;
                }

                var columns = attributes.GetNamedItem("Columns").Value.TryParseInt32(0);
                var rows = attributes.GetNamedItem("Rows").Value.TryParseInt32(0);
                var height = attributes.GetNamedItem("Height").Value.TryParseInt32(0);
                var width = attributes.GetNamedItem("Width").Value.TryParseInt32(0);
                var topOffset = attributes.GetNamedItem("TopOffset").Value.TryParseInt32(0);
                var leftOffset = attributes.GetNamedItem("LeftOffset").Value.TryParseInt32(0);
                var item = attributes.GetNamedItem("IsUnlocked");
                var isUnlocked = item == null ? true : bool.Parse(item.Value);

                var mappedChannels = new List<PixelMapping>();
                foreach (XmlNode mappedNode in node.ChildNodes)
                {
                    IPixel pixel;
                    var channelNode = mappedNode.FirstChild;
                    if (channelNode == null)
                    {
                        pixel = new EmptyPixel();
                    }
                    else
                    {
                        var type = channelNode.Attributes.GetNamedItem("Type").Value;
                        switch (type)
                        {
                            case "Single":
                                var channelIdValue = channelNode.GetAttributeValue("ChannelId");
                                var channelId = string.IsNullOrEmpty(channelIdValue) ? string.Empty : channelIdValue;
                                var color =
                                    (Color)
                                    ColorConverter.ConvertFromString(channelNode.Attributes.GetNamedItem("Color").Value);
                                pixel = new SingleColorPixel(
                                    this._channels.FirstOrDefault(x => x.Name == channelId), color);
                                break;
                            case "RGB":
                                channelIdValue = channelNode.GetAttributeValue("RedChannel");
                                var redChannelId = string.IsNullOrEmpty(channelIdValue)
                                                      ? string.Empty : channelIdValue;
                                var redChannel = this._channels.FirstOrDefault(x => x.Name == redChannelId);
                                channelIdValue = channelNode.GetAttributeValue("GreenChannel");
                                var blueChannelId = string.IsNullOrEmpty(channelIdValue)
                                                        ? string.Empty : channelIdValue;
                                var greenChannel = this._channels.FirstOrDefault(x => x.Name == blueChannelId);
                                channelIdValue = channelNode.GetAttributeValue("BlueChannel");
                                var greenChannelId = string.IsNullOrEmpty(channelIdValue)
                                                         ? string.Empty : channelIdValue;
                                var blueChannel = this._channels.FirstOrDefault(x => x.Name == greenChannelId);
                                pixel = new RedGreenBluePixel(redChannel, greenChannel, blueChannel);
                                break;
                            case "RGBW":
                                channelIdValue = channelNode.GetAttributeValue("RedChannel");
                                redChannelId = string.IsNullOrEmpty(channelIdValue) ? string.Empty : channelIdValue;
                                redChannel = this._channels.FirstOrDefault(x => x.Name == redChannelId);
                                channelIdValue = channelNode.GetAttributeValue("GreenChannel");
                                blueChannelId = string.IsNullOrEmpty(channelIdValue) ? string.Empty : channelIdValue;
                                greenChannel = this._channels.FirstOrDefault(x => x.Name == blueChannelId);
                                channelIdValue = channelNode.GetAttributeValue("BlueChannel");
                                greenChannelId = string.IsNullOrEmpty(channelIdValue) ? string.Empty : channelIdValue;
                                blueChannel = this._channels.FirstOrDefault(x => x.Name == greenChannelId);
                                channelIdValue = channelNode.GetAttributeValue("WhiteChannel");
                                var whiteChannelId = string.IsNullOrEmpty(channelIdValue)
                                                         ? string.Empty : channelIdValue;
                                var whiteChannel = this._channels.FirstOrDefault(x => x.Name == whiteChannelId);
                                pixel = new RedGreenBlueWhitePixel(redChannel, greenChannel, blueChannel, whiteChannel);
                                break;
                            default:
                                pixel = new EmptyPixel();
                                break;
                        }
                    }

                    var mappedChannel = new PixelMapping(pixel);
                    mappedChannels.Add(mappedChannel);
                }

                var displayElement = new DisplayElement(
                    columns, rows, height, leftOffset, topOffset, width, mappedChannels, isUnlocked)
                                     {
                                         Name = attributes.GetNamedItem("Name").Value
                                     };
                this._elements.Add(displayElement);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}