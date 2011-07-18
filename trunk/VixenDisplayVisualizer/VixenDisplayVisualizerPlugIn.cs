namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Xml;
    using Vixen.PlugIns.VixenDisplayVisualizer.Channels;
    using Vixen.PlugIns.VixenDisplayVisualizer.Dialogs;
    using Vixen.PlugIns.VixenDisplayVisualizer.ViewModels;
    using MessageBox = System.Windows.MessageBox;

    public class VixenDisplayVisualizerPlugIn : IEventDrivenOutputPlugIn
    {
        private readonly List<Channel> _channels = new List<Channel>();
        private readonly List<DisplayElement> _elements = new List<DisplayElement>();
        private DisplayVisualizer _displayVisualizer;
        private int _pluginChannelsFrom;
        private int _pluginChannelsTo;
        private SetupData _setupData;
        private Setup _setupDialog;
        private XmlNode _setupNode;

        public string Author
        {
            get
            {
                return "Erik Mathisen - erik@mathisen.us";
            }
        }

        public string Description
        {
            get
            {
                return "Vixen Display Visualizer Plugin - Allows for the virtual creation of a display, and playing a sequence on it.";
            }
        }

        public HardwareMap[] HardwareMap
        {
            get
            {
                return new HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Vixen Display Visualizer";
            }
        }

        public void Event(byte[] channelValues)
        {
            if (((_displayVisualizer != null) && !_displayVisualizer.Disposing)
                && !_displayVisualizer.IsDisposed)
            {
                _displayVisualizer.UpdateWith(channelValues);
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            _channels.Clear();
            _channels.AddRange(executableObject.Channels);
            _setupData = setupData;
            _setupNode = setupNode;
            LoadDataFromSetupNode();

            ////_setupData.GetBytes(_setupNode, "BackgroundImage", new byte[0]);
        }

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

        public void Startup()
        {
            if (_channels.Any())
            {
                LoadDataFromSetupNode();

                ////var system = (ISystem)Interfaces.Available["ISystem"];
                ////var constructor = typeof(DisplayVisualizer).GetConstructor(new[] { typeof(VisualizerViewModel) });
                var viewModel = new VisualizerViewModel(_channels, _elements);

                ////var form = system..InstantiateForm(constructor, new object[] { viewModel });
                _displayVisualizer = new DisplayVisualizer(viewModel);
                _displayVisualizer.Show();
            }
        }

        public void Setup()
        {
            if (_channels.Any())
            {
                LoadDataFromSetupNode();

                var viewModel = new SetupViewModel();
                _channels.ForEach(x => viewModel.Channels.Add(x));
                _elements.ForEach(x => viewModel.DisplayElements.Add(x));
                bool saveData;
                using (_setupDialog = new Setup(viewModel))
                {
                    _setupDialog.ShowDialog();
                    _elements.Clear();
                    _elements.AddRange(viewModel.DisplayElements);
                    saveData = true;
                }

                if (saveData)
                {
                    while (_setupNode.ChildNodes.Count > 0)
                    {
                        _setupNode.RemoveChild(_setupNode.ChildNodes[0]);
                    }

                    foreach (var element in _elements)
                    {
                        var node = _setupNode.OwnerDocument.CreateElement("DisplayElement");
                        node.AppendAttribute("Rows", element.Rows.ToString());
                        node.AppendAttribute("Columns", element.Columns.ToString());
                        node.AppendAttribute("Height", element.Height.ToString());
                        node.AppendAttribute("Width", element.Width.ToString());
                        node.AppendAttribute("LeftOffset", element.LeftOffset.ToString());
                        node.AppendAttribute("TopOffset", element.TopOffset.ToString());
                        node.AppendAttribute("Name", element.Name);
                        foreach (var mappedChannel in element.MappedChannels)
                        {
                            var mappedNode = node.OwnerDocument.CreateElement("MappedChannel");

                            var channel = mappedChannel.Channel;
                            if (channel != null)
                            {
                                var channelNode = mappedNode.OwnerDocument.CreateElement("Channel");
                                mappedNode.AppendChild(channelNode);
                                if (channel is EmptyChannel)
                                {
                                    channelNode.AppendAttribute("Type", "Empty");
                                }
                                else if (channel is SingleColorChannel)
                                {
                                    channelNode.AppendAttribute("Type", "Single");
                                    var singleColorChannel = (SingleColorChannel)channel;
                                    var vixenChannel = singleColorChannel.Channel;
                                    channelNode.AppendAttribute("ChannelId", vixenChannel == null ? string.Empty : vixenChannel.ID.ToString());
                                    channelNode.AppendAttribute("Color", singleColorChannel.DisplayColor.ToString());
                                }
                                else
                                {
                                    var rgb = channel as RedGreenBlueChannel;

                                    var redChannel = rgb.RedChannel;
                                    channelNode.AppendAttribute("RedChannel", redChannel == null ? string.Empty : redChannel.ID.ToString());
                                    var greenChannel = rgb.GreenChannel;
                                    channelNode.AppendAttribute("GreenChannel", greenChannel == null ? string.Empty : greenChannel.ID.ToString());
                                    var blueChannel = rgb.BlueChannel;
                                    channelNode.AppendAttribute("BlueChannel", blueChannel == null ? string.Empty : blueChannel.ID.ToString());

                                    var rgbw = channel as RedGreenBlueWhiteChannel;
                                    var type = "RGB";
                                    if (rgbw != null)
                                    {
                                        var whiteChannel = rgbw.WhiteChannel;
                                        channelNode.AppendAttribute("WhiteChannel", whiteChannel == null ? string.Empty : whiteChannel.ID.ToString());
                                        type += "W";
                                    }

                                    channelNode.AppendAttribute("Type", type);
                                }
                            }

                            node.AppendChild(mappedNode);
                        }

                        _setupNode.AppendChild(node);
                    }

                    LoadDataFromSetupNode();
                }

                _setupDialog = null;
            }
            else
            {
                MessageBox.Show("There are no channels assigned to this plugin.", Name, MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void LoadDataFromSetupNode()
        {
            // get the attribute collection and the from/to attributes if available
            var setupNodeAttributes = _setupNode.Attributes;
            var fromAttribute = setupNodeAttributes.GetNamedItem("from");
            var toAttribute = setupNodeAttributes.GetNamedItem("to");

            // if we got both attributes, try and parse them
            if (fromAttribute != null
                && toAttribute != null)
            {
                // try to parse both attributes
                _pluginChannelsFrom = fromAttribute.Value.TryParseInt32(0);
                _pluginChannelsTo = toAttribute.Value.TryParseInt32(0);

                // if either is zero, make both zero to indicate not setup
                if (_pluginChannelsFrom == 0
                    || _pluginChannelsTo == 0)
                {
                    _pluginChannelsFrom = 0;
                    _pluginChannelsTo = 0;
                }
            }

            _elements.Clear();
            foreach (XmlNode node in _setupNode.ChildNodes)
            {
                var attributes = node.Attributes;
                if (attributes == null)
                {
                    continue;
                }

                var columns = attributes.GetNamedItem("Columns").Value.TryParseInt32(0);
                var rows = attributes.GetNamedItem("Rows").Value.TryParseInt32(0);
                var height = attributes.GetNamedItem("Height").Value.TryParseInt32(0);
                var width = attributes.GetNamedItem("Width").Value.TryParseInt32(0);
                var topOffset = attributes.GetNamedItem("TopOffset").Value.TryParseInt32(0);
                var leftOffset = attributes.GetNamedItem("LeftOffset").Value.TryParseInt32(0);

                var mappedChannels = new List<MappedChannel>();
                foreach (XmlNode mappedNode in node.ChildNodes)
                {
                    IChannel channel;
                    var channelNode = mappedNode.FirstChild;
                    if (channelNode == null)
                    {
                        channel = new EmptyChannel();
                    }
                    else
                    {
                        var type = channelNode.Attributes.GetNamedItem("Type").Value;
                        switch (type)
                        {
                            case "Single":
                                var channelIdValue = channelNode.GetAttributeValue("ChannelId");
                                var channelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                var color = (Color)ColorConverter.ConvertFromString(channelNode.Attributes.GetNamedItem("Color").Value);
                                channel = new SingleColorChannel(_channels.FirstOrDefault(x => x.ID == channelId), color);
                                break;
                            case "RGB":
                                channelIdValue = channelNode.GetAttributeValue("RedChannel");
                                var redChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                var redChannel = _channels.FirstOrDefault(x => x.ID == redChannelId);
                                channelIdValue = channelNode.GetAttributeValue("GreenChannel");
                                var blueChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                var greenChannel = _channels.FirstOrDefault(x => x.ID == blueChannelId);
                                channelIdValue = channelNode.GetAttributeValue("BlueChannel");
                                var greenChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                var blueChannel = _channels.FirstOrDefault(x => x.ID == greenChannelId);
                                channel = new RedGreenBlueChannel(redChannel, greenChannel, blueChannel);
                                break;
                            case "RGBW":
                                channelIdValue = channelNode.GetAttributeValue("RedChannel");
                                redChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                redChannel = _channels.FirstOrDefault(x => x.ID == redChannelId);
                                channelIdValue = channelNode.GetAttributeValue("GreenChannel");
                                blueChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                greenChannel = _channels.FirstOrDefault(x => x.ID == blueChannelId);
                                channelIdValue = channelNode.GetAttributeValue("BlueChannel");
                                greenChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                blueChannel = _channels.FirstOrDefault(x => x.ID == greenChannelId);
                                channelIdValue = channelNode.GetAttributeValue("WhiteChannel");
                                var whiteChannelId = string.IsNullOrEmpty(channelIdValue) ? 0 : ulong.Parse(channelIdValue);
                                var whiteChannel = _channels.FirstOrDefault(x => x.ID == whiteChannelId);
                                channel = new RedGreenBlueWhiteChannel(redChannel, greenChannel, blueChannel, whiteChannel);
                                break;
                            default:
                                channel = new EmptyChannel();
                                break;
                        }
                    }

                    var mappedChannel = new MappedChannel(channel);
                    mappedChannels.Add(mappedChannel);
                }

                var displayElement = new DisplayElement(columns, rows, height, leftOffset, topOffset, width, mappedChannels);
                displayElement.Name = attributes.GetNamedItem("Name").Value;
                _elements.Add(displayElement);
            }
        }
    }
}
