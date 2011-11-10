//=====================================================================
//	SetupForm.cs - the setup dialog form
//		version 1.0.0.1 - 2 june 2010
//=====================================================================

//=====================================================================
// Copyright (c) 2010 Joshua 1 Systems Inc. All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//    1. Redistributions of source code must retain the above copyright notice, this list of
//       conditions and the following disclaimer.
//    2. Redistributions in binary form must reproduce the above copyright notice, this list
//       of conditions and the following disclaimer in the documentation and/or other materials
//       provided with the distribution.
// THIS SOFTWARE IS PROVIDED BY JOSHUA 1 SYSTEMS INC. "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of Joshua 1 Systems Inc.
//=====================================================================

namespace E131_VixenPlugin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Windows.Forms;
    using J1Sys;
    using VixenModules.Controller.E131;
    using Help = VixenModules.Controller.E131.Help;

    public class SetupForm : Form
    {
        // column indexes - must be changed if column addrange code is changed
        // could refactor to a variable and initialize it at column add time
        // but then it wouldn't work well with switch/case code
        private const int ACTIVE_COLUMN = 0;
        private const int DESTINATION_COLUMN = 4;
        private const int SIZE_COLUMN = 3;
        private const int START_COLUMN = 2;
        private const int TTL_COLUMN = 5;
        private const int UNIVERSE_COLUMN = 1;

        // plugin channel count as set by vixen
        private readonly SortedList<string, int> badIDs = new SortedList<string, int>();
        private readonly SortedList<string, int> multicasts = new SortedList<string, int>();
        private readonly SortedDictionary<string, string> nicIDs = new SortedDictionary<string, string>();
        private readonly SortedDictionary<string, string> nicNames = new SortedDictionary<string, string>();
        private readonly SortedList<string, int> unicasts = new SortedList<string, int>();

        // datagridview columns
        private DataGridViewCheckBoxColumn activeColumn;
        private Button cancelButton;
        private IContainer components;
        private DataGridViewComboBoxColumn destinationColumn;
        private ContextMenuStrip destinationContextMenuStrip;
        private ToolTip destinationToolTip;

        // other entry controls
        private TextBox eventRepeatCountTextBox;

        // our buttons
        private Button okButton;
        private int pluginChannelCount;

        // common contextmenustrip for row manipulation - added to most of the columns
        private ContextMenuStrip rowManipulationContextMenuStrip = new ContextMenuStrip();
        private DataGridViewTextBoxColumn sizeColumn;
        private DataGridViewTextBoxColumn startColumn;
        private CheckBox statisticsCheckBox;
        private DataGridViewTextBoxColumn ttlColumn;
        private DataGridViewNumbered univDGVN;

        // universe datagridview cell event arguments to track mouse entry
        private DataGridViewCellEventArgs univDGVNCellEventArgs;
        private DataGridViewTextBoxColumn universeColumn;
        private CheckBox warningsCheckBox;

        // destination column tooltip and contextmenustrip (to make right-click work)

        // -------------------------------------------------------------
        // 	SetupForm() - our constructor
        // 		build some nic tables and initialize the component
        // -------------------------------------------------------------

        public SetupForm()
        {
            // first build some sorted lists and dictionaries for the nics

            // get all the nics
            var nics = NetworkInterface.GetAllNetworkInterfaces();

            // do we have a nics?
            if (nics != null)
            {
                // anything in there?
                if (nics.Length > 0)
                {
                    // then iterate through them
                    foreach (var nic in nics)
                    {
                        // if not a tunnel
                        if (nic.NetworkInterfaceType.CompareTo(NetworkInterfaceType.Tunnel) != 0)
                        {
                            // and supports multicast
                            if (nic.SupportsMulticast)
                            {
                                // then add it to multicasts table by name
                                multicasts.Add(nic.Name, 0);

                                // add it to available nicIDs table
                                nicIDs.Add(nic.Id, nic.Name);

                                // add ot to available nicNames table
                                nicNames.Add(nic.Name, nic.Id);
                            }
                        }
                    }
                }
            }

            // finally initialize the form
            InitializeComponent();
        }

        public int EventRepeatCount
        {
            get
            {
                int count;

                if (!Int32.TryParse(eventRepeatCountTextBox.Text, out count))
                {
                    count = 0;
                }

                return count;
            }

            set
            {
                eventRepeatCountTextBox.Text = value.ToString();
            }
        }

        // -------------------------------------------------------------
        // 	Dispose() - our dispose
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	PluginChannelCount - property to expose channel count
        // -------------------------------------------------------------
        public int PluginChannelCount
        {
            set
            {
                pluginChannelCount = value;
            }
        }

        public bool StatisticsOption
        {
            get
            {
                return statisticsCheckBox.Checked;
            }

            set
            {
                statisticsCheckBox.Checked = value;
            }
        }

        // -------------------------------------------------------------
        // 	UniverseCount - property to expose universe count
        // -------------------------------------------------------------
        public int UniverseCount
        {
            get
            {
                return univDGVN.Rows.Count;
            }
        }

        public bool WarningsOption
        {
            get
            {
                return warningsCheckBox.Checked;
            }

            set
            {
                warningsCheckBox.Checked = value;
            }
        }

        // -------------------------------------------------------------
        // 	UniverseClear() - clear the rows in the datagridview
        // 		probably never needed
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	UniverseAdd() - add a row from config to rows and tables
        // -------------------------------------------------------------

        public bool UniverseAdd(bool active, int universe, int start, int size, string unicast, string multicast, int ttl)
        {
            string destination = null;

            // if it is unicast we add the destination to the
            // drop down list if it isn't already there
            // and we 'reformat' to text for display

            if (unicast != null)
            {
                if (!unicasts.ContainsKey(unicast))
                {
                    unicasts.Add(unicast, 0);
                    destinationColumn.Items.Add("Unicast " + unicast);
                }

                destination = "Unicast " + unicast;
            }

            // if it is multicast we check for the id to match
            // a nic. if it doesn't we warn of interface changes
            // and store in bad id's so we only warn once

            if (multicast != null)
            {
                if (nicIDs.ContainsKey(multicast))
                {
                    destination = "Multicast " + nicIDs[multicast];
                }
                else
                {
                    if (!badIDs.ContainsKey(multicast))
                    {
                        badIDs.Add(multicast, 0);
                        MessageBox.Show(
                                        "Warning - Interface IDs have changed. Please reselect all empty destinations.", 
                                        "Network Interface Mapping", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Warning);
                    }
                }
            }

            // all set, add the row - convert int's to strings ourselves
            univDGVN.Rows.Add(
                              new object[]
                              {
                                 active, universe.ToString(), start.ToString(), size.ToString(), destination, ttl.ToString() 
                              });
            return true;
        }

        public void UniverseClear()
        {
            univDGVN.Rows.Clear();
        }

        // -------------------------------------------------------------
        // 	UniverseGet() - allow referenced retrieval of the data
        // -------------------------------------------------------------

        public bool UniverseGet(
            int index, 
            ref bool active, 
            ref int universe, 
            ref int start, 
            ref int size, 
            ref string unicast, 
            ref string multicast, 
            ref int ttl)
        {
            var row = univDGVN.Rows[index];

            if (row.IsNewRow)
            {
                return false;
            }

            if (row.Cells[ACTIVE_COLUMN].Value == null)
            {
                active = false;
            }
            else
            {
                active = (bool)row.Cells[ACTIVE_COLUMN].Value;
            }

            // all numeric columns are stored as strings
            universe = Extensions.TryParseInt32((string)row.Cells[UNIVERSE_COLUMN].Value, 1);
            start = Extensions.TryParseInt32((string)row.Cells[START_COLUMN].Value, 1);
            size = Extensions.TryParseInt32((string)row.Cells[SIZE_COLUMN].Value, 1);
            ttl = Extensions.TryParseInt32((string)row.Cells[TTL_COLUMN].Value, 1);

            // first set both unicast and multicast results to null
            unicast = null;
            multicast = null;

            // then set the selected unicast/multicast destination

            if (row.Cells[DESTINATION_COLUMN].Value != null)
            {
                var destination = (string)row.Cells[DESTINATION_COLUMN].Value;

                if (destination.StartsWith("Unicast "))
                {
                    unicast = destination.Substring(8);
                }
                else if (destination.StartsWith("Multicast "))
                {
                    multicast = nicNames[destination.Substring(10)];
                }
            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void AddUnicastIP()
        {
            var unicastForm = new UnicastForm();

            if (univDGVN.CurrentCell != null)
            {
                if (univDGVN.CurrentCell.IsInEditMode)
                {
                    univDGVN.EndEdit();
                }
            }

            if (unicastForm.ShowDialog()
                == DialogResult.OK)
            {
                IPAddress ipAddress;
                bool valid;

                valid = IPAddress.TryParse(unicastForm.IPAddrText, out ipAddress);

                if (valid)
                {
                    var ipBytes = ipAddress.GetAddressBytes();

                    if (ipBytes[0] == 0 && ipBytes[1] == 0 && ipBytes[2] == 0
                        && ipBytes[3] == 0)
                    {
                        valid = false;
                    }

                    if (ipBytes[0] == 255 && ipBytes[1] == 255 && ipBytes[2] == 255
                        && ipBytes[3] == 255)
                    {
                        valid = false;
                    }
                }

                if (!valid)
                {
                    MessageBox.Show("Error - Invalid IP Address", "IP Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var ipAddressText = ipAddress.ToString();

                    if (unicasts.ContainsKey(ipAddressText))
                    {
                        MessageBox.Show(
                                        "Error - Duplicate IP Address", 
                                        "IP Address Validation", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Error);
                    }
                    else
                    {
                        unicasts.Add(ipAddressText, 0);
                        SetDestinations();
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            MainMenu mainMenu;
            MenuItem mIHelp, mIHelpAbout, mIHelpShowSys;
            Label label;

            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
            Text = "J1Sys E1.31 Setup Form";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(800, 450);

            SuspendLayout();

            rowManipulationContextMenuStrip = new ContextMenuStrip();
            rowManipulationContextMenuStrip.Opening += rowManipulationContextMenuStrip_Opening;
            rowManipulationContextMenuStrip.Items.Add("Insert Row", null, univDGVN_InsertRow);
            rowManipulationContextMenuStrip.Items.Add("Delete Row", null, univDGVN_DeleteRow);
            rowManipulationContextMenuStrip.Items.Add("-");
            rowManipulationContextMenuStrip.Items.Add("Move Row Up", null, univDGVN_MoveRowUp);
            rowManipulationContextMenuStrip.Items.Add("Move Row Down", null, univDGVN_MoveRowDown);

            mIHelpAbout = new MenuItem("&About J1Sys E1.31...", Help.AboutClick);
            mIHelpShowSys = new MenuItem("&Show System Info", Help.ShowSysClick);
            mIHelp = new MenuItem("&Help", new[] { mIHelpAbout, mIHelpShowSys });

            mainMenu = new MainMenu(new[] { mIHelp });

            Menu = mainMenu;

            univDGVN = new DataGridViewNumbered();
            univDGVN.Name = "UniverseDGV";
            univDGVN.Location = new Point(10, 10);
            univDGVN.Size = new Size(600, 230);
            univDGVN.BackgroundColor = BackColor;
            univDGVN.BorderStyle = BorderStyle.None;
            univDGVN.RowHeadersVisible = true;
            univDGVN.SelectionMode = DataGridViewSelectionMode.CellSelect;
            univDGVN.AllowUserToAddRows = true;
            univDGVN.AllowUserToDeleteRows = false;
            univDGVN.AutoSize = false;
            univDGVN.TabIndex = 1;
            univDGVN.CellMouseEnter += univDGVN_CellMouseEnter;
            univDGVN.CellMouseClick += univDGVN_CellMouseClick;
            univDGVN.CellValidating += univDGVN_CellValidating;
            univDGVN.CellEndEdit += univDGVN_CellEndEdit;
            univDGVN.CellEnter += univDGVN_CellEnter;
            univDGVN.DefaultValuesNeeded += univDGVN_DefaultValuesNeeded;
            univDGVN.EditingControlShowing += univDGVN_EditingControlShowing;

            activeColumn = new DataGridViewCheckBoxColumn();
            activeColumn.Width = 25;
            activeColumn.HeaderText = "Act";
            activeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            activeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            activeColumn.ReadOnly = false;
            activeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            activeColumn.Resizable = DataGridViewTriState.False;
            activeColumn.ContextMenuStrip = rowManipulationContextMenuStrip;

            universeColumn = new DataGridViewTextBoxColumn();
            universeColumn.Width = 60;
            universeColumn.HeaderText = "Universe";
            universeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            universeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            universeColumn.ReadOnly = false;
            universeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            universeColumn.HeaderCell.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            universeColumn.Resizable = DataGridViewTriState.False;
            universeColumn.ContextMenuStrip = rowManipulationContextMenuStrip;
            universeColumn.MaxInputLength = 5;

            startColumn = new DataGridViewTextBoxColumn();
            startColumn.Width = 60;
            startColumn.HeaderText = "Start";
            startColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            startColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            startColumn.ReadOnly = false;
            startColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            startColumn.HeaderCell.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            startColumn.Resizable = DataGridViewTriState.False;
            startColumn.ContextMenuStrip = rowManipulationContextMenuStrip;
            startColumn.MaxInputLength = 5;

            sizeColumn = new DataGridViewTextBoxColumn();
            sizeColumn.Width = 60;
            sizeColumn.HeaderText = "Size";
            sizeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            sizeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            sizeColumn.ReadOnly = false;
            sizeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            sizeColumn.HeaderCell.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            sizeColumn.Resizable = DataGridViewTriState.False;
            sizeColumn.ContextMenuStrip = rowManipulationContextMenuStrip;
            sizeColumn.MaxInputLength = 3;

            destinationColumn = new DataGridViewComboBoxColumn();
            destinationColumn.Width = 300;
            destinationColumn.HeaderText = "Destination";
            destinationColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            destinationColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            destinationColumn.ReadOnly = false;
            destinationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            destinationColumn.HeaderCell.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            destinationColumn.Resizable = DataGridViewTriState.False;
            destinationColumn.CellTemplate.ToolTipText = "RightClick to add a new Unicast IP Address";
            SetDestinations();

            ttlColumn = new DataGridViewTextBoxColumn();
            ttlColumn.Width = 30;
            ttlColumn.HeaderText = "TTL";
            ttlColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            ttlColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ttlColumn.ReadOnly = false;
            ttlColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            ttlColumn.Resizable = DataGridViewTriState.False;
            ttlColumn.ContextMenuStrip = rowManipulationContextMenuStrip;
            ttlColumn.MaxInputLength = 2;

            univDGVN.Columns.AddRange(
                                      new DataGridViewColumn[]
                                      {
                                         activeColumn, universeColumn, startColumn, sizeColumn, destinationColumn, ttlColumn 
                                      });

            Controls.Add(univDGVN);

            warningsCheckBox = new CheckBox();
            warningsCheckBox.Location = new Point(10, 260);
            warningsCheckBox.AutoSize = true;
            warningsCheckBox.Text = "Display ALL Warnings/Errors and wait For OK";
            Controls.Add(warningsCheckBox);

            statisticsCheckBox = new CheckBox();
            statisticsCheckBox.Location = new Point(10, 284);
            statisticsCheckBox.AutoSize = true;
            statisticsCheckBox.Text = "Gather statistics and display at end of session";
            Controls.Add(statisticsCheckBox);

            eventRepeatCountTextBox = new TextBox();
            eventRepeatCountTextBox.Location = new Point(10, 308);
            eventRepeatCountTextBox.Width = 30;
            eventRepeatCountTextBox.TextAlign = HorizontalAlignment.Right;
            eventRepeatCountTextBox.MaxLength = 2;
            eventRepeatCountTextBox.KeyPress += NumTextBox_KeyPress;
            eventRepeatCountTextBox.Validating += eventRepeatCountTextBox_Validating;
            Controls.Add(eventRepeatCountTextBox);

            label = new Label();
            label.Location = new Point(60, 308);
            label.AutoSize = true;
            label.Text =
                "Event Repeat Count: Set to 0 to send all events to each universe,\r\nset to > 0 to skip 'x' events if data is unchanged on a per universe basis.";
            Controls.Add(label);

            // add our buttons centered at the bottom
            okButton = new Button();
            okButton.Name = "okButton";
            okButton.AutoSize = true;
            okButton.TabIndex = 101;
            okButton.Text = "&OK";
            okButton.Location = new Point(ClientSize.Width / 2 - okButton.Width - 10, ClientSize.Height - okButton.Height - 25);
            okButton.Click += okButton_Click;
            Controls.Add(okButton);

            cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.AutoSize = true;
            cancelButton.TabIndex = 102;
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(ClientSize.Width / 2 + 10, ClientSize.Height - cancelButton.Height - 25);
            Controls.Add(cancelButton);
            CancelButton = cancelButton;

            ResumeLayout(true);
            Application.DoEvents();
        }

        // -------------------------------------------------------------
        // 	NumTextBox_KeyPress() - event handler for a numeric textbox
        // 		this handler is used by the univDVGN editing control
        // 		for the numeric columns and by a simple textbox control
        // 		for numeric only input controls
        // -------------------------------------------------------------
        private void NumTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }

            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }

            if (e.Handled)
            {
                MessageBeepClass.MessageBeep(MessageBeepClass.BeepType.SimpleBeep);
            }
        }

        private void SetDestinations()
        {
            destinationColumn.Items.Clear();

            foreach (var destination in multicasts.Keys)
            {
                destinationColumn.Items.Add("Multicast " + destination);
            }

            foreach (var ipAddr in unicasts.Keys)
            {
                destinationColumn.Items.Add("Unicast " + ipAddr);
            }
        }

        private void destinationContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            AddUnicastIP();
        }

        private void eventRepeatCountTextBox_Validating(object sender, CancelEventArgs e)
        {
            var count = 0;

            if (!Int32.TryParse(((TextBox)sender).Text, out count))
            {
                count = 0;
            }

            if (count < 0
                || 99 < count)
            {
                e.Cancel = true;
            }

            if (e.Cancel)
            {
                MessageBeepClass.MessageBeep(MessageBeepClass.BeepType.SimpleBeep);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var valid = true;
            var errorList = new StringBuilder();
            var universeDestinations = new SortedList<string, int>();

            // first buid a table of active universe/destination combos

            foreach (DataGridViewRow row in univDGVN.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                if (row.Cells[ACTIVE_COLUMN].Value != null)
                {
                    if ((bool)row.Cells[ACTIVE_COLUMN].Value)
                    {
                        if (row.Cells[DESTINATION_COLUMN].Value != null)
                        {
                            var universeDestination = (string)row.Cells[UNIVERSE_COLUMN].Value + ":"
                                                      + (string)row.Cells[DESTINATION_COLUMN].Value;
                            if (universeDestinations.ContainsKey(universeDestination))
                            {
                                universeDestinations[universeDestination] = 1;
                            }
                            else
                            {
                                universeDestinations.Add(universeDestination, 0);
                            }
                        }
                    }
                }
            }

            // now scan for empty destinations, duplicate universe/destination combos, channels errors, etc.

            foreach (DataGridViewRow row in univDGVN.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                // only test if row is active
                if (row.Cells[ACTIVE_COLUMN].Value != null)
                {
                    if ((bool)row.Cells[ACTIVE_COLUMN].Value)
                    {
                        // test for null destinations
                        if (row.Cells[DESTINATION_COLUMN].Value == null)
                        {
                            if (!valid)
                            {
                                errorList.Append("\r\n");
                            }

                            errorList.Append("Row ");
                            errorList.Append((row.Index + 1).ToString());
                            errorList.Append(": No Destination Selected");
                            valid = false;
                        }
                        else
                        {
                            // otherwise, test for duplicate universe/destination combos
                            var universeDestination = (string)row.Cells[UNIVERSE_COLUMN].Value + ":"
                                                      + (string)row.Cells[DESTINATION_COLUMN].Value;

                            if (universeDestinations[universeDestination] != 0)
                            {
                                if (!valid)
                                {
                                    errorList.Append("\r\n");
                                }

                                errorList.Append("Row ");
                                errorList.Append((row.Index + 1).ToString());
                                errorList.Append(": Duplicate Universe/Destination Combination");
                                valid = false;
                            }
                        }

                        // only test for range if more than 0 channels, otherwise wait for runtime
                        if (pluginChannelCount > 0)
                        {
                            // now test for valid channel start
                            if (Extensions.TryParseInt32((string)row.Cells[START_COLUMN].Value, 1) > pluginChannelCount)
                            {
                                if (!valid)
                                {
                                    errorList.Append("\r\n");
                                }

                                errorList.Append("Row ");
                                errorList.Append((row.Index + 1).ToString());
                                errorList.Append(": Start Channel Out Of Range");
                                valid = false;
                            }

                            // now test for valid channel size
                            if (Extensions.TryParseInt32((string)row.Cells[START_COLUMN].Value, 1)
                                + Extensions.TryParseInt32((string)row.Cells[SIZE_COLUMN].Value, 1) - 1 > pluginChannelCount)
                            {
                                if (!valid)
                                {
                                    errorList.Append("\r\n");
                                }

                                errorList.Append("Row ");
                                errorList.Append((row.Index + 1).ToString());
                                errorList.Append(": Start Channel + Size Out Of Range");
                                valid = false;
                            }
                        }

                        // now test for ttl value
                        if (Extensions.TryParseInt32((string)row.Cells[TTL_COLUMN].Value, 1) == 0)
                        {
                            if (!valid)
                            {
                                errorList.Append("\r\n");
                            }

                            errorList.Append("Row ");
                            errorList.Append((row.Index + 1).ToString());
                            errorList.Append(": Warning - Zero TTL");
                            valid = false;
                        }
                    }
                }
            }

            if (!valid)
            {
                if (
                    J1MsgBox.ShowMsg(
                                     "Your configurations contains active entries that may cause run time errors.\r\n\r\nHit OK to continue and save your configuration. Hit Cancel to re-edit before saving.", 
                                     errorList.ToString(), 
                                     "Configuration Validation", 
                                     MessageBoxButtons.OKCancel, 
                                     MessageBoxIcon.Error)
                    == DialogResult.OK)
                {
                    valid = true;
                }
            }

            if (valid)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        // -------------------------------------------------------------
        // 	univDGCN_DefaultValuesNeeded()
        // 		initialize an empty row
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	rowManipulationContextMenuStrip_Opening()
        // 		we need to gray out a few items based on row, or if
        // 		it is the 'adding' row cancel the menu
        // -------------------------------------------------------------

        private void rowManipulationContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var contextMenuStrip = sender as ContextMenuStrip;

            if (contextMenuStrip != null)
            {
                if (univDGVNCellEventArgs != null)
                {
                    var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

                    if (row.IsNewRow)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        // enable/disable move row up
                        contextMenuStrip.Items[3].Enabled = row.Index != 0;

                        // disable move row down
                        contextMenuStrip.Items[4].Enabled = false;
                        // enable move row down if able
                        if (row.Index
                            < univDGVN.Rows.Count - 1)
                        {
                            if (!univDGVN.Rows[row.Index + 1].IsNewRow)
                            {
                                contextMenuStrip.Items[4].Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        // -------------------------------------------------------------
        // 	univDGN_CellEnter() - cell enter event
        // 		we just use this for destination column to issue
        // 		a BeginEdit(). we feel this makes combobox more
        // 		user friendly.
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	univDGVN_CellEndEdit() - clear the errortext
        // -------------------------------------------------------------
        private void univDGVN_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            univDGVN.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void univDGVN_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DESTINATION_COLUMN)
            {
                univDGVN.BeginEdit(false);
            }
        }

        // -------------------------------------------------------------
        // 	univDGVN_CellValidating() - validate the cell
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	univDGVN_CellMouseClick() - cell mouse click event
        // -------------------------------------------------------------

        private void univDGVN_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // if it's the headers - sort 'em

            if (e.RowIndex
                == -1)
            {
                if (0 < e.ColumnIndex
                    && e.ColumnIndex < 5)
                {
                    var lsd = ListSortDirection.Ascending;

                    if (e.Button
                        == MouseButtons.Right)
                    {
                        lsd = ListSortDirection.Descending;
                    }

                    univDGVN.Sort(univDGVN.Columns[e.ColumnIndex], lsd);
                }
            }

                // if it's the rows - handle specials
            else
            {
                // if it's the right button
                if (e.Button
                    == MouseButtons.Right)
                {
                    // if it's the destination column - they want to add a unicast ip
                    if (e.ColumnIndex == DESTINATION_COLUMN)
                    {
                        AddUnicastIP();
                    }
                }
            }
        }

        private void univDGVN_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            univDGVNCellEventArgs = e;
        }

        private void univDGVN_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cellValue = e.FormattedValue;
            var cellValueText = cellValue as string;
            var cellValueInt = 0;

            if (cellValueText != null)
            {
                if (!Int32.TryParse(cellValueText, out cellValueInt))
                {
                    cellValueInt = 0;
                }
            }

            switch (e.ColumnIndex)
            {
                case UNIVERSE_COLUMN:
                    if (cellValueText == null)
                    {
                        e.Cancel = true;
                    }
                    else if (cellValueInt < 1
                             || 64000 < cellValueInt)
                    {
                        e.Cancel = true;
                    }

                    if (e.Cancel)
                    {
                        univDGVN.Rows[e.RowIndex].ErrorText = "Universe must be between 1 and 64000 inclusive";
                    }

                    break;

                case START_COLUMN:
                    if (cellValueText == null)
                    {
                        e.Cancel = true;
                    }
                    else if (cellValueInt < 1
                             || 99999 < cellValueInt)
                    {
                        e.Cancel = true;
                    }

                    if (e.Cancel)
                    {
                        univDGVN.Rows[e.RowIndex].ErrorText = "Start must be between 1 and 99999 inclusive";
                    }

                    break;

                case SIZE_COLUMN:
                    if (cellValueText == null)
                    {
                        e.Cancel = true;
                    }
                    else if (cellValueInt < 1
                             || 512 < cellValueInt)
                    {
                        e.Cancel = true;
                    }

                    if (e.Cancel)
                    {
                        univDGVN.Rows[e.RowIndex].ErrorText = "Size must be between 1 and 512 inclusive";
                    }

                    break;

                case TTL_COLUMN:
                    if (cellValueText == null)
                    {
                        e.Cancel = true;
                    }
                    else if (cellValueInt < 0
                             || 99 < cellValueInt)
                    {
                        e.Cancel = true;
                    }

                    if (e.Cancel)
                    {
                        univDGVN.Rows[e.RowIndex].ErrorText = "TTL must be between 0 and 99 inclusive";
                    }

                    break;
            }

            if (e.Cancel)
            {
                MessageBox.Show(
                                univDGVN.Rows[e.RowIndex].ErrorText, 
                                "Cell Validation", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Exclamation);
            }
        }

        private void univDGVN_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[ACTIVE_COLUMN].Value = false;
            e.Row.Cells[UNIVERSE_COLUMN].Value = "1";
            e.Row.Cells[START_COLUMN].Value = "1";
            e.Row.Cells[SIZE_COLUMN].Value = "1";
            e.Row.Cells[TTL_COLUMN].Value = "1";
        }

        private void univDGVN_DeleteRow(object sender, EventArgs e)
        {
            if (univDGVNCellEventArgs != null)
            {
                var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

                if (!row.IsNewRow)
                {
                    univDGVN.Rows.RemoveAt(row.Index);
                }
            }
        }

        private void univDGVN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var columnIndex = univDGVN.CurrentCell.ColumnIndex;

            if (columnIndex == UNIVERSE_COLUMN || columnIndex == START_COLUMN || columnIndex == SIZE_COLUMN
                || columnIndex == TTL_COLUMN)
            {
                // first remove the event handler (if previously added)
                e.Control.KeyPress -= NumTextBox_KeyPress;

                // now add our event handler
                e.Control.KeyPress += NumTextBox_KeyPress;
            }

            if (columnIndex == DESTINATION_COLUMN)
            {
                var control = e.Control as DataGridViewComboBoxEditingControl;

                if (control != null)
                {
                    if (destinationToolTip == null)
                    {
                        destinationToolTip = new ToolTip();
                    }

                    destinationToolTip.SetToolTip(e.Control, "RightClick to add a new Unicast IP Address");

                    if (destinationContextMenuStrip == null)
                    {
                        destinationContextMenuStrip = new ContextMenuStrip();
                        destinationContextMenuStrip.Opening += destinationContextMenuStrip_Opening;
                    }

                    control.ContextMenuStrip = destinationContextMenuStrip;
                }
            }
        }

        private void univDGVN_InsertRow(object sender, EventArgs e)
        {
            if (univDGVNCellEventArgs != null)
            {
                var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

                if (!row.IsNewRow)
                {
                    univDGVN.Rows.Insert(row.Index, new object[] { false, "1", "1", "1", null, "1" });
                }
            }
        }

        private void univDGVN_MoveRowDown(object sender, EventArgs e)
        {
            if (univDGVNCellEventArgs != null)
            {
                var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];
                var rowIndex = row.Index;

                if (!row.IsNewRow)
                {
                    if (rowIndex < univDGVN.Rows.Count - 1)
                    {
                        if (!univDGVN.Rows[rowIndex + 1].IsNewRow)
                        {
                            univDGVN.Rows.RemoveAt(rowIndex);
                            univDGVN.Rows.Insert(rowIndex + 1, row);
                        }
                    }
                }
            }
        }

        private void univDGVN_MoveRowUp(object sender, EventArgs e)
        {
            if (univDGVNCellEventArgs != null)
            {
                var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];
                var rowIndex = row.Index;

                if (!row.IsNewRow
                    && rowIndex > 0)
                {
                    univDGVN.Rows.RemoveAt(rowIndex);
                    univDGVN.Rows.Insert(rowIndex - 1, row);
                }
            }
        }

        // -------------------------------------------------------------
        // 	AddUnicastIP()
        // -------------------------------------------------------------

        // -------------------------------------------------------------
        // 	UnicastForm() - form to get a new unicast ip address
        // -------------------------------------------------------------

        private class UnicastForm : Form
        {
            private Button cancelButton;

            private IContainer components;
            private IPTextBox ipTextBox;
            private Button okButton;

            public UnicastForm()
            {
                InitializeComponent();
            }

            public string IPAddrText
            {
                get
                {
                    return ipTextBox.Text;
                }

                set
                {
                    ipTextBox.Text = value;
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }

                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                components = new Container();
                AutoScaleMode = AutoScaleMode.Font;
                Text = "Unicast IP Address Form";
                StartPosition = FormStartPosition.CenterParent;
                Size = new Size(300, 150);

                SuspendLayout();

                ipTextBox = new IPTextBox();
                ipTextBox.Location = new Point(10, 10);
                ipTextBox.Font = Font;
                ipTextBox.Text = string.Empty;
                Controls.Add(ipTextBox);

                okButton = new Button();
                okButton.DialogResult = DialogResult.OK;
                okButton.Name = "okButton";
                okButton.AutoSize = true;
                okButton.TabIndex = 101;
                okButton.Text = "&OK";
                okButton.Location = new Point(ClientSize.Width / 2 - okButton.Width - 10, ClientSize.Height - okButton.Height - 25);
                Controls.Add(okButton);

                cancelButton = new Button();
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Name = "cancelButton";
                cancelButton.AutoSize = true;
                cancelButton.TabIndex = 102;
                cancelButton.Text = "&Cancel";
                cancelButton.Location = new Point(ClientSize.Width / 2 + 10, ClientSize.Height - cancelButton.Height - 25);
                Controls.Add(cancelButton);
                CancelButton = cancelButton;
                ResumeLayout(true);
                Application.DoEvents();
            }
        }
    }
}
