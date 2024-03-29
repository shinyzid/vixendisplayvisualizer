﻿//=====================================================================
//
//	OutputPlugin - E1.31 Plugin for Vixen 3.0
//
//		The original base code was generated by Visual Studio based
//		on the interface specification intrinsic to the Vixen plugin
//		technology. All other comments and code are the work of the
//		author. Some comments are based on the fundamental work
//		gleaned from published works by others in the Vixen community
//		including those of Jonathon Reinhart.
//
//=====================================================================

//=====================================================================
//
// Copyright (c) 2010 Joshua 1 Systems Inc. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//    1. Redistributions of source code must retain the above copyright notice, this list of
//       conditions and the following disclaimer.
//
//    2. Redistributions in binary form must reproduce the above copyright notice, this list
//       of conditions and the following disclaimer in the documentation and/or other materials
//       provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY JOSHUA 1 SYSTEMS INC. "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of Joshua 1 Systems Inc.
//
//=====================================================================

namespace VixenModules.Controller.E131
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    using Vixen.Commands;
    using Vixen.Module.Output;

    using VixenModules.Controller.E131.J1Sys;

    // -----------------------------------------------------------------
    // 
    // OutputPlugin - the output plugin class for vixen
    // 
    // -----------------------------------------------------------------
    public class E131OutputPlugin : OutputModuleInstanceBase
    {
        // our option settings
        private int _eventCnt;

        private int _eventRepeatCount;

        private Guid _guid;

        // a stringbuilder to store warnings, errors, and statistics
        private StringBuilder _messageTexts;

        // a table of UniverseEntry objects to hold all universes

        // a sorted list of NetworkInterface object to use for sockets
        private SortedList<string, NetworkInterface> _nicTable;

        // plugin wide statistics

        // plugin information supplied by vixen (by xml)
        private int _pluginChannelsFrom;

        private int _pluginChannelsTo;

        private byte _seqNum; // should this be changed to per universe?

        private bool _statisticsOption;

        private long _totalTicks;

        private List<UniverseEntry> _universeTable;

        private bool _warningsOption;

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            // load all of our xml into working objects
            this.LoadSetupNodeInfo();

            // find all of the network interfaces & build a sorted list indexed by Id
            this._nicTable = new SortedList<string, NetworkInterface>();

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in nics)
            {
                if (nic.NetworkInterfaceType.CompareTo(NetworkInterfaceType.Tunnel) != 0)
                {
                    this._nicTable.Add(nic.Id, nic);
                }
            }
        }

        // -------------------------------------------------------------
        // 
        // 	Setup() - called when the user has requested to setup
        // 			  the plugin instance
        // 
        // -------------------------------------------------------------
        public override bool Setup()
        {
            // define/create objects
            XmlElement newChild;
            using (var setupForm = new SetupForm())
            {
                this.LoadSetupNodeInfo();

                // if our channels from/to are setup then tell the setupForm
                if (this._pluginChannelsFrom != 0 && this._pluginChannelsTo != 0)
                {
                    setupForm.PluginChannelCount = this._pluginChannelsTo - this._pluginChannelsFrom + 1;
                }

                // for each universe add it to setup form
                foreach (var uE in this._universeTable)
                {
                    setupForm.UniverseAdd(
                        uE.Active, uE.Universe, uE.Start + 1, uE.Size, uE.Unicast, uE.Multicast, uE.Ttl);
                }

                setupForm.WarningsOption = this._warningsOption;
                setupForm.StatisticsOption = this._statisticsOption;
                setupForm.EventRepeatCount = this._eventRepeatCount;

                if (setupForm.ShowDialog() == DialogResult.OK)
                {
                    // first get rid of our old children
                    while (_setupNode.ChildNodes.Count > 0)
                    {
                        _setupNode.RemoveChild(_setupNode.ChildNodes[0]);
                    }

                    // add the Guid child
                    newChild = _setupNode.OwnerDocument.CreateElement("Guid");
                    newChild.SetAttribute("id", this._guid.ToString());
                    _setupNode.AppendChild(newChild);

                    // add the Options child
                    newChild = _setupNode.OwnerDocument.CreateElement("Options");
                    newChild.SetAttribute("warnings", setupForm.WarningsOption.ToString());
                    newChild.SetAttribute("statistics", setupForm.StatisticsOption.ToString());
                    newChild.SetAttribute("eventRepeatCount", setupForm.EventRepeatCount.ToString());
                    _setupNode.AppendChild(newChild);

                    // add each of the universes as a child
                    for (int i = 0; i < setupForm.UniverseCount; i++)
                    {
                        bool active = true;
                        int universe = 0;
                        int start = 0;
                        int size = 0;
                        string unicast = string.Empty;
                        string multicast = string.Empty;
                        int ttl = 0;

                        if (setupForm.UniverseGet(
                            i, ref active, ref universe, ref start, ref size, ref unicast, ref multicast, ref ttl))
                        {
                            newChild = _setupNode.OwnerDocument.CreateElement("Universe");

                            newChild.SetAttribute("active", active.ToString());
                            newChild.SetAttribute("number", universe.ToString());
                            newChild.SetAttribute("start", start.ToString());
                            newChild.SetAttribute("size", size.ToString());
                            if (unicast != null)
                            {
                                newChild.SetAttribute("unicast", unicast);
                            }
                            else if (multicast != null)
                            {
                                newChild.SetAttribute("multicast", multicast);
                            }

                            newChild.SetAttribute("ttl", ttl.ToString());

                            _setupNode.AppendChild(newChild);
                        }
                    }

                    // update in memory table to match xml
                    this.LoadSetupNodeInfo();
                }
            }
        }

        // -------------------------------------------------------------
        // 
        // 	Shutdown() - called when execution is stopped or the
        // 				 plugin instance is no longer going to be
        // 				 referenced
        // 
        // -------------------------------------------------------------
        public void Shutdown()
        {
            // keep track of interface ids we have shutdown
            var idList = new SortedList<string, int>();

            // iterate through universetable
            foreach (var uE in this._universeTable)
            {
                // assume multicast
                string id = uE.Multicast;

                // if unicast use psuedo id
                if (uE.Unicast != null)
                {
                    id = "unicast";
                }

                // if active
                if (uE.Active)
                {
                    // and a usable socket
                    if (uE.Socket != null)
                    {
                        // if not already done
                        if (!idList.ContainsKey(id))
                        {
                            // record it & shut it down
                            idList.Add(id, 1);
                            uE.Socket.Shutdown(SocketShutdown.Both);
                            uE.Socket.Close();
                        }
                    }
                }
            }

            if (this._statisticsOption)
            {
                if (this._messageTexts.Length > 0)
                {
                    this._messageTexts.AppendLine();
                }

                this._messageTexts.AppendLine("Events: " + this._eventCnt);
                this._messageTexts.AppendLine(
                    "Total Time: " + this._totalTicks + " Ticks;  " + TimeSpan.FromTicks(this._totalTicks).Milliseconds
                    + " ms");

                foreach (var uE in this._universeTable)
                {
                    if (uE.Active)
                    {
                        this._messageTexts.AppendLine();
                        this._messageTexts.Append(uE.StatsToText);
                    }
                }

                J1MsgBox.ShowMsg(
                    "Plugin Statistics:", 
                    this._messageTexts.ToString(), 
                    "J1Sys E1.31 Vixen Plugin", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        // -------------------------------------------------------------
        // 
        // 	Startup() - called when a sequence is executed
        // 
        // 
        // 	todo:
        // 
        // 		1) probably add error checking on all 'new' operations
        // 		and system calls
        // 
        // 		2) better error reporting and logging
        // 	
        // -------------------------------------------------------------
        public void Startup()
        {
            // working copy of networkinterface object
            NetworkInterface networkInterface;

            // a single socket to use for unicast (if needed)
            Socket unicastSocket = null;

            // working ipaddress object
            IPAddress ipAddress = null;

            // a sortedlist containing the multicast sockets we've already done
            var nicSockets = new SortedList<string, Socket>();

            // reload all of our xml into working objects
            this.LoadSetupNodeInfo();

            // initialize plugin wide stats
            this._eventCnt = 0;
            this._totalTicks = 0;

            // initialize sequence # for E1.31 packet (should it be per universe?)
            this._seqNum = 0;

            // initialize messageTexts stringbuilder to hold all warnings/errors
            this._messageTexts = new StringBuilder();

            // check for configured from/to
            if (this._pluginChannelsFrom == 0 && this._pluginChannelsTo == 0)
            {
                foreach (var uE in this._universeTable)
                {
                    uE.Active = false;
                }

                this._messageTexts.AppendLine("Plugin Channels From/To Configuration Error!!");
            }

            // now we need to scan the universeTable
            foreach (var uE in this._universeTable)
            {
                // active? - check universeentry start and size
                if (uE.Active)
                {
                    // is start out of range?
                    if (this._pluginChannelsFrom + uE.Start > this._pluginChannelsTo)
                    {
                        this._messageTexts.AppendLine("Start Error - " + uE.InfoToText);
                        uE.Active = false;
                    }

                    // is size (end) out of range?
                    if (this._pluginChannelsFrom + uE.Start + uE.Size - 1 > this._pluginChannelsTo)
                    {
                        this._messageTexts.AppendLine("Start/Size Error - " + uE.InfoToText);
                        uE.Active = false;
                    }
                }

                // if it's still active we'll look into making a socket for it
                if (uE.Active)
                {
                    // if it's unicast it's fairly easy to do
                    if (uE.Unicast != null)
                    {
                        // is this the first unicast universe?
                        if (unicastSocket == null)
                        {
                            // yes - make a new socket to use for ALL unicasts
                            unicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        }

                        // use the common unicastsocket
                        uE.Socket = unicastSocket;

                        // try to parse our ip address
                        if (!IPAddress.TryParse(uE.Unicast, out ipAddress))
                        {
                            // oops - bad ip, fuss and deactivate
                            uE.Active = false;
                            uE.Socket = null;
                            this._messageTexts.AppendLine(
                                "Invalid Unicast IP: " + uE.Unicast + " - " + uE.RowUnivToText);
                        }
                        else
                        {
                            // if good, make our destination endpoint
                            uE.DestIpEndPoint = new IPEndPoint(ipAddress, 5568);
                        }
                    }

                    // if it's multicast roll up your sleeves we've got work to do
                    if (uE.Multicast != null)
                    {
                        // create an ipaddress object based on multicast universe ip rules
                        var multicastIpAddress =
                            new IPAddress(new byte[] { 239, 255, (byte)(uE.Universe >> 8), (byte)(uE.Universe & 0xff) });

                        // create an ipendpoint object based on multicast universe ip/port rules
                        var multicastIpEndPoint = new IPEndPoint(multicastIpAddress, 5568);

                        // first check for multicast id in nictable
                        if (!this._nicTable.ContainsKey(uE.Multicast))
                        {
                            // no - deactivate and scream & yell!!
                            uE.Active = false;
                            this._messageTexts.AppendLine(
                                "Invalid Multicast NIC ID: " + uE.Multicast + " - " + uE.RowUnivToText);
                        }
                        else
                        {
                            // yes - let's get a working networkinterface object
                            networkInterface = this._nicTable[uE.Multicast];

                            // have we done this multicast id before?
                            if (nicSockets.ContainsKey(uE.Multicast))
                            {
                                // yes - easy to do - use existing socket
                                uE.Socket = nicSockets[uE.Multicast];

                                // setup destipendpoint based on multicast universe ip rules
                                uE.DestIpEndPoint = multicastIpEndPoint;
                            }

                                // is the interface up?
                            else if (networkInterface.OperationalStatus != OperationalStatus.Up)
                            {
                                // no - deactivate and scream & yell!!
                                uE.Active = false;
                                this._messageTexts.AppendLine(
                                    "Multicast Interface Down: " + networkInterface.Name + " - " + uE.RowUnivToText);
                            }
                            else
                            {
                                // new interface in 'up' status - let's make a new udp socket
                                uE.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                                // get a working copy of ipproperties
                                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                                // get a working copy of all unicasts
                                UnicastIPAddressInformationCollection unicasts = ipProperties.UnicastAddresses;

                                ipAddress = null;

                                foreach (var unicast in unicasts)
                                {
                                    if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                                    {
                                        ipAddress = unicast.Address;
                                    }
                                }

                                if (ipAddress == null)
                                {
                                    this._messageTexts.AppendLine(
                                        "No IP On Multicast Interface: " + networkInterface.Name + " - " + uE.InfoToText);
                                }
                                else
                                {
                                    // set the multicastinterface option
                                    uE.Socket.SetSocketOption(
                                        SocketOptionLevel.IP, 
                                        SocketOptionName.MulticastInterface, 
                                        ipAddress.GetAddressBytes());

                                    // set the multicasttimetolive option
                                    uE.Socket.SetSocketOption(
                                        SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, uE.Ttl);

                                    // setup destipendpoint based on multicast universe ip rules
                                    uE.DestIpEndPoint = multicastIpEndPoint;

                                    // add this socket to the socket table for reuse
                                    nicSockets.Add(uE.Multicast, uE.Socket);
                                }
                            }
                        }
                    }

                    // if still active we need to create an empty packet
                    if (uE.Active)
                    {
                        var zeroBfr = new byte[uE.Size];
                        var e131Packet = new E131Packet(this._guid, string.Empty, 0, (ushort)uE.Universe, zeroBfr, 0, uE.Size);
                        uE.PhyBuffer = e131Packet.PhyBuffer;
                    }
                }
            }

            // any warnings/errors recorded?
            if (this._messageTexts.Length > 0)
            {
                // should we display them
                if (this._warningsOption)
                {
                    // show our warnings/errors
                    J1MsgBox.ShowMsg(
                        "The following warnings and errors were detected during startup:", 
                        this._messageTexts.ToString(), 
                        "Startup Warnings/Errors", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Exclamation);

                    // discard warning/errors after reporting them
                    this._messageTexts = new StringBuilder();
                }
            }

            // MessageBox.Show("Startup");
#if VIXEN21
			return new List<Form> {};
#endif
        }

        private E131ModuleDataModel GetDataModel()
        {
            return (E131ModuleDataModel)this.ModuleData;
        }

        protected override void _SetOutputCount(int outputCount)
        {
        }

        protected override void _UpdateState(Command[] outputStates)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            var channelValues = outputStates.ToChannelValuesAsBytes();
            this._eventCnt++;

            foreach (var uE in this._universeTable)
            {
                if (uE.Active)
                {
                    if (this._eventRepeatCount > 0)
                    {
                        if (uE.EventRepeatCount-- > 0)
                        {
                            if (E131Packet.CompareSlots(uE.PhyBuffer, channelValues, uE.Start, uE.Size))
                            {
                                continue;
                            }
                        }
                    }

                    E131Packet.CopySeqNumSlots(uE.PhyBuffer, channelValues, uE.Start, uE.Size, this._seqNum++);
                    uE.Socket.SendTo(uE.PhyBuffer, uE.DestIpEndPoint);
                    uE.EventRepeatCount = this._eventRepeatCount;

                    uE.PktCount++;
                    uE.SlotCount += uE.Size;
                }
            }

            stopWatch.Stop();

            this._totalTicks += stopWatch.ElapsedTicks;
        }

        private void LoadSetupNodeInfo()
        {
            int rowNum = 1;

            this._universeTable = new List<UniverseEntry>();

            // init from/to to indicate not setup
            this._pluginChannelsFrom = 0;
            this._pluginChannelsTo = 0;
            this._warningsOption = true;
            this._statisticsOption = true;
            this._eventRepeatCount = 0;

            this._guid = Guid.Empty;

            foreach (XmlNode child in _setupNode.ChildNodes)
            {
                XmlAttributeCollection attributes = child.Attributes;
                XmlNode attribute;

                if (child.Name == "Guid")
                {
                    if ((attribute = attributes.GetNamedItem("id")) != null)
                    {
                        try
                        {
                            this._guid = new Guid(attribute.Value);
                        }
                        catch
                        {
                            this._guid = Guid.Empty;
                        }
                    }
                }

                if (child.Name == "Options")
                {
                    this._warningsOption = false;
                    if ((attribute = attributes.GetNamedItem("warnings")) != null)
                    {
                        if (attribute.Value == "True")
                        {
                            this._warningsOption = true;
                        }
                    }

                    this._statisticsOption = false;
                    if ((attribute = attributes.GetNamedItem("statistics")) != null)
                    {
                        if (attribute.Value == "True")
                        {
                            this._statisticsOption = true;
                        }
                    }

                    this._eventRepeatCount = 0;
                    if ((attribute = attributes.GetNamedItem("eventRepeatCount")) != null)
                    {
                        this._eventRepeatCount = attribute.Value.TryParseInt32(0);
                    }
                }

                if (child.Name == "Universe")
                {
                    bool active = false;
                    int universe = 1;
                    int start = 1;
                    int size = 1;
                    string unicast = null;
                    string multicast = null;
                    int ttl = 1;

                    if ((attribute = attributes.GetNamedItem("active")) != null)
                    {
                        if (attribute.Value == "True")
                        {
                            active = true;
                        }
                    }

                    if ((attribute = attributes.GetNamedItem("number")) != null)
                    {
                        universe = attribute.Value.TryParseInt32(1);
                    }

                    if ((attribute = attributes.GetNamedItem("start")) != null)
                    {
                        start = attribute.Value.TryParseInt32(1);
                    }

                    if ((attribute = attributes.GetNamedItem("size")) != null)
                    {
                        size = attribute.Value.TryParseInt32(1);
                    }

                    if ((attribute = attributes.GetNamedItem("unicast")) != null)
                    {
                        unicast = attribute.Value;
                    }

                    if ((attribute = attributes.GetNamedItem("multicast")) != null)
                    {
                        multicast = attribute.Value;
                    }

                    if ((attribute = attributes.GetNamedItem("ttl")) != null)
                    {
                        ttl = attribute.Value.TryParseInt32(1);
                    }

                    this._universeTable.Add(
                        new UniverseEntry(rowNum++, active, universe, start - 1, size, unicast, multicast, ttl));
                }
            }

            if (this._guid == Guid.Empty)
            {
                this._guid = Guid.NewGuid();
            }
        }
    }
}