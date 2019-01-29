/* ----------------------------------------------------------------------------- 
* Windows Events Providers Explorer / GUI
* Copyright (c) Elias Bachaalany <lallousz-x86@yahoo.com>
* All rights reserved.
* 
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
* 
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
* ----------------------------------------------------------------------------- 
*
*
* 01/19/2016 - Initial version
* 01/20/2016 - Added "File/Clear cache"
* 01/26/2016 - Added Template fields filter
*            - Implemented Message content filter
*            - Implemented "Copy provider name", "IDs", and "IDs as case" functionalities
* 01/27/2016 - Added "Delete" context menu to the provider metadata list. This will help filtering the output.
*            - Added keyboard shortcuts
*            - More filter options for the provider template fields
* 03/16/2016 - Bumped to version 1.2
*            - Added name/guid provider filter
*            - Added Copy provider GUID
* 03/17/2016 - Format Keyword flag as hexadecimal
* 03/20/2016 - v1.2.1
*            - Added Keywords listview / "Copy Flags"
*
TODO
------

- Start using FluentLib.NET instead
- Investigate how to improve the cli utility, I feel it is blocked / has bugs or missing information.

*/
using WEPExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Explore
{
    public partial class WEPExplorer : Form
    {
        [Flags]
        public enum CtxMenuFlags: int
        {
            Delete = 0x01,
            Find = 0x02,
            Export = 0x04,
            All = 0xff,
            AllNoDelete = All & ~Delete,
        }

        #region XML consts
        public const string XML_CHANNEL = "Channel";
        public const string XML_CHANNELS = "Channels";
        public const string XML_EVENT_METADATA = "EventMetadata";
        public const string XML_GUID = "Guid";
        public const string XML_HELPLINK = "HelpLink";
        public const string XML_ID = "Id";
        public const string XML_EVENT = "Event";
        public const string XML_IMPORTED = "Imported";
        public const string XML_INDEX = "Index";
        public const string XML_KEYWORD = "Keyword";
        public const string XML_KEYWORDS = "Keywords";
        public const string XML_LEVEL = "Level";
        public const string XML_LEVELS = "Levels";
        public const string XML_MESSAGE = "Message";
        public const string XML_MESSAGEFILEPATH = "MessageFilePath";
        public const string XML_METADATA = "Metadata";
        public const string XML_NAME = "Name";
        public const string XML_OPCODE = "Opcode";
        public const string XML_OPCODES = "Opcodes";
        public const string XML_PARAMETERFILEPATH = "ParameterFilePath";
        public const string XML_PATH = "Path";
        public const string XML_PROVIDER = "Provider";
        public const string XML_PROVIDERS = "Providers";
        public const string XML_PUBLISHER_MESSAGE = "PublisherMessage";
        public const string XML_RESOURCEFILEPATH = "ResourceFilePath";
        public const string XML_TASK = "Task";
        public const string XML_TASKS = "Tasks";
        public const string XML_TEMPLATE = "Template";
        public const string XML_VALUE = "Value";
        public const string XML_VERSION = "Version";
        #endregion
        const string FILTER_TEXT_Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
        const string FILTER_TEXT_DefaultExt = "txt";

        public class FormSettings
        {
            public string LastProviderName;
            public int FormWidth = -1;
            public int FormHeight = -1;
        }

        private FormSettings settings = new FormSettings();

        private string ProvidersFileName;
        private static string STR_NULL_GUID = Guid.Empty.ToString("B");

        class ContextMenuTag
        {
            public ListView lv;
            public string FindString;

            public delegate void OnLvItemAction(
                ContextMenuTag Ctx,
                object Item);

            public int SearchPos = 0;
        }

        private static string ProvidersMetadataFolder = "Providers";

        public class ProvidersFilter
        {
            public XmlNode Nodes;
            public string ProviderString;
        }

        public class ProviderMetadataFilter
        {
            public XmlNode Nodes;
            public HashSet<string> Channels;
            public HashSet<string> Tasks;
            public HashSet<string> Levels;
            public HashSet<string> Opcodes;
            public HashSet<string> TemplateFields;
            public int TemplateFieldCondition; // 0 = ALL, 1 = ANYOF
            public string Message;
        }

        public ProviderMetadataFilter LastMetadataFilter = null;
        private ProvidersFilter LastProvidersFilter;
        public const string STR_TITLE = "Windows Events Providers Explorer - v1.2.1 - lallouslab.net";

        #region Common UI
        private ContextMenuTag CreateCommonMenuItems(
            ContextMenuStrip Menu,
            ListView LV,
            CtxMenuFlags MFlags = CtxMenuFlags.All)
        {
            // FindFirst
            var FindFirst = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | System.Windows.Forms.Keys.F),
                Size = new System.Drawing.Size(202, 22),
                Text = "Find",
            };
            FindFirst.Click += new EventHandler(menuCommonLVFindFirst_Click);

            // FindNext
            var FindNext = new ToolStripMenuItem()
            {
                ShortcutKeys = Keys.F3,
                Size = new System.Drawing.Size(202, 22),
                Text = "Find Next",
            };
            FindNext.Click += new EventHandler(menuCommonLVFindNext_Click);

            var CopyItem = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.C),
                Size = new System.Drawing.Size(202, 22),
                Text = "Copy"
            };
            CopyItem.Click += new EventHandler(menuCommonLVCopyItem_Click);

            var SelectAll = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.A),
                Size = new System.Drawing.Size(202, 22),
                Text = "Select all"
            };
            SelectAll.Click += new EventHandler(menuCommonLVSelectAll_Click);

            var ExportToTextFile = new ToolStripMenuItem()
            {
                ShortcutKeys = (Keys.Control | Keys.S),
                Size = new System.Drawing.Size(202, 22),
                Text = "Export to text file"
            };
            ExportToTextFile.Click += new EventHandler(menuCommonLVExportToTextFile_Click);

            var Delete = new ToolStripMenuItem()
            {
                ShortcutKeys = Keys.Delete,
                Size = new System.Drawing.Size(202, 22),
                Text = "Delete"
            };
            Delete.Click += new EventHandler(menuCommonLVDeleteItem_Click);

            if (MFlags.HasFlag(CtxMenuFlags.Find))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    FindFirst,
                    FindNext,
                });
            }

            if (MFlags.HasFlag(CtxMenuFlags.Delete))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    Delete
                });
            }

            if (MFlags.HasFlag(CtxMenuFlags.Export))
            {
                Menu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    ExportToTextFile
                });
            }

            Menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                CopyItem,
                SelectAll,
            });

            // Install column sorter
            LV.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lvCommon_ColumnClick);

            var Context = new ContextMenuTag()
            {
                lv = LV
            };
            Menu.Tag = Context;
            LV.Tag = Context;

            LV.ContextMenuStrip = Menu;

            return Context;
        }

        private void menuCommonLVCopyItem_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var sb = new StringBuilder();
            foreach (ListViewItem lvi in lvCtx.lv.SelectedItems)
                sb.AppendLine(lvi.GetItemsString(Join: " "));

            Clipboard.Clear();
            Clipboard.SetText(sb.ToString());
        }

        private void menuCommonLVDeleteItem_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            lvCtx.lv.BeginUpdate();
            foreach (ListViewItem lvi in lvCtx.lv.SelectedItems)
                lvCtx.lv.Items.Remove(lvi);

            lvCtx.lv.EndUpdate();
        }

        private void lvCommon_ColumnClick(
           object sender,
           ColumnClickEventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var lv = lvCtx.lv;

            SimpleListViewItemComparer sorter = lv.ListViewItemSorter as SimpleListViewItemComparer;

            if (sorter == null)
            {
                sorter = new SimpleListViewItemComparer(e.Column);
                lv.ListViewItemSorter = sorter;
            }
            else
            {
                sorter.Column = e.Column;
            }

            sorter.Ascending = !sorter.Ascending;

            lv.Sort();
        }

        private ContextMenuTag GetCommonLVContext(object sender)
        {
            object Tag = null;
            if (sender is ToolStripMenuItem)
                Tag = (sender as ToolStripMenuItem).Owner.Tag;
            else if (sender is ListView)
                Tag = (sender as ListView).Tag;

            var cmt = Tag as ContextMenuTag;
            return cmt;
        }

        private void menuCommonLVFindFirst_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            string str = InputString(lvCtx.FindString, "Find");
            if (string.IsNullOrEmpty(str))
                return;

            lvCtx.SearchPos = 0;
            lvCtx.FindString = str;
            menuCommonLVFindNext_Click(sender, e);
        }

        private void menuCommonLVFindNext_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            if (string.IsNullOrEmpty(lvCtx.FindString))
                return;

            for (int i = lvCtx.SearchPos, c = lvCtx.lv.Items.Count; i < c; i++)
            {
                ListViewItem lvi = lvCtx.lv.Items[i];
                foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
                {
                    lvi.Selected = false;
                    if (lvsi.Text.IndexOf(lvCtx.FindString, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        lvi.Selected = true;
                        lvi.EnsureVisible();
                        lvCtx.lv.FocusedItem = lvi;

                        lvCtx.SearchPos = i + 1;
                        return;
                    }
                }
            }

            // Nothing found, reset search position
            lvCtx.SearchPos = 0;
        }

        private void menuCommonLVSelectAll_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx != null)
                lvCtx.lv.SelectAll();
        }

        private void menuCommonLVExportToTextFile_Click(
            object sender,
            EventArgs e)
        {
            var lvCtx = GetCommonLVContext(sender);
            if (lvCtx == null)
                return;

            var lv = lvCtx.lv;

            string fn = BrowseForFile(
                Utils.GetCurrentAsmDirectory(),
                "",
                FILTER_TEXT_DefaultExt,
                FILTER_TEXT_Filter,
                true);
            if (string.IsNullOrEmpty(fn))
                return;

            using (TextWriter Out = new System.IO.StreamWriter(fn, false))
            {
                // Write column header
                foreach (ColumnHeader Cur in lv.Columns)
                {
                    Out.Write("\"" + Cur.Text + "\"");
                    Out.Write("\t");
                }
                Out.WriteLine();

                foreach (ListViewItem Item in lv.Items)
                    Out.WriteLine(Item.GetItemsString());

                Out.Close();
            }
        }

        private static string BrowseForFile(
            string DefaultDir,
            string DefaultFile,
            string DefExt,
            string Filter,
            bool bSaveDialog = false)
        {
            FileDialog FD;
            if (bSaveDialog)
                FD = new SaveFileDialog();
            else
                FD = new OpenFileDialog();

            FD.DefaultExt = DefExt;
            FD.Filter = Filter;
            FD.FileName = DefaultFile;
            if (!string.IsNullOrEmpty(DefaultDir))
                FD.InitialDirectory = DefaultDir;

            return (FD.ShowDialog() == DialogResult.OK) ? FD.FileName : null;
        }

        private static string BrowseForFolder(string DefaultDir)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = DefaultDir;

            return (FBD.ShowDialog() == DialogResult.OK) ? FBD.SelectedPath : null;
        }

        static private void MsgBoxError(
            string Message, 
            params string[] args)
        {
            MessageBox.Show(
                string.Format(Message, args),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        static private void MsgBoxInfo(
            string Message, 
            params string[] args)
        {
            MessageBox.Show(
                string.Format(Message, args),
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static string InputString(
            string DefVal,
            string Title)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter value",
                Title,
                DefVal,
                -1,
                -1);

            if (string.IsNullOrEmpty(input))
                return null;

            return input;
        }
        #endregion

        public WEPExplorer()
        {
            InitializeComponent();

            InitPathes();
        }

        private void WEPExplorerForm_Load(
            object sender,
            EventArgs e)
        {
            Text = STR_TITLE;

            cbProviderMetadataTemplateFieldsMatchCondition.SelectedIndex = 0;

            CreateCommonMenuItems(
                ctxmenuProviders,
                lvProviders,
                CtxMenuFlags.AllNoDelete);

            CreateCommonMenuItems(
                ctxmenuProvKeywords,
                lvProviderKeywords,
                CtxMenuFlags.AllNoDelete);

            CreateCommonMenuItems(
                ctxmenuProvMetadata,
                lvProviderMetadata);

            // Populate providers
            LastProvidersFilter = new ProvidersFilter();
            GetProviders(out LastProvidersFilter.Nodes);
            PopulateProviders(LastProvidersFilter);
        }

        private void InitPathes()
        {
            if (!Directory.Exists(ProvidersMetadataFolder))
                Directory.CreateDirectory(ProvidersMetadataFolder);

            ProvidersFileName = Path.Combine(ProvidersMetadataFolder, "All.xml");
        }

        private void GetProviders(out XmlNode Nodes)
        {
            if (!File.Exists(ProvidersFileName))
                Cli.GetProviders(ProvidersFileName);

            XmlDocument xd = new XmlDocument();
            xd.Load(ProvidersFileName);

            Nodes = xd.DocumentElement;
        }

        private string GetProviderMetadataFile(string ProviderName)
        {
            return Path.Combine(ProvidersMetadataFolder, ProviderName + ".xml");
        }

        private XmlNode GetProviderMetadataXml(string ProviderName)
        {
            string ProviderMetadataFileName = GetProviderMetadataFile(ProviderName);

            if (!File.Exists(ProviderMetadataFileName))
            {
                if (!Cli.GetProviderMetadata(ProviderName, ProviderMetadataFileName))
                    return null;
            }

            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(ProviderMetadataFileName);

                return xd.DocumentElement;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void ComputeProviderMetadata(
            string ProviderName,
            out XmlNode Nodes)
        {
            // Remember provider name
            settings.LastProviderName = ProviderName;

            // Retrieve the metadata
            Nodes = GetProviderMetadataXml(ProviderName);

            //
            // Populate the combo checkboxes
            //
            PopulateCbChkFilters(
                cbchkChannels,
                Nodes.SelectNodes(string.Format("{0}/{1}/{2}/{3}", 
                        XML_PROVIDER, 
                        XML_METADATA, 
                        XML_CHANNELS, 
                        XML_CHANNEL)),
                XML_MESSAGE,
                XML_PATH);

            PopulateCbChkFilters(
                cbchkLevels,
                Nodes.SelectNodes(string.Format("{0}/{1}/{2}/{3}", 
                        XML_PROVIDER, 
                        XML_METADATA, 
                        XML_LEVELS, 
                        XML_LEVEL)),
                XML_MESSAGE);

            PopulateCbChkFilters(
                cbchkOpcodes,
                Nodes.SelectNodes(string.Format("{0}/{1}/{2}/{3}", 
                        XML_PROVIDER, 
                        XML_METADATA, 
                        XML_OPCODES, 
                        XML_OPCODE)),
                XML_MESSAGE,
                XML_NAME);

            PopulateCbChkFilters(
                cbchkTasks,
                Nodes.SelectNodes(string.Format("{0}/{1}/{2}/{3}", XML_PROVIDER, XML_METADATA, XML_TASKS, XML_TASK)),
                XML_MESSAGE,
                XML_NAME);

            PopulateCbChkTemplateFieldsFilter(
                cbchkTemplateFields, 
                Nodes);
        }

        private string xnGetText(XmlNode xnNode, string NodeName)
        {
            var xn = xnNode.SelectSingleNode(NodeName);
            if (xn == null)
                return string.Empty;
            else
                return xn.InnerText;
        }

        private void ApplyProviderNameFilter()
        {
            if (LastProvidersFilter == null)
                return;

            LastProvidersFilter.ProviderString = txtProviderNameGuidFilter.Text;
            PopulateProviders(LastProvidersFilter);
        }

        private void txtProviderNameGuidFilter_TextChanged(
            object sender,
            EventArgs e)
        {
            ApplyProviderNameFilter();
        }

        private void PopulateProviders(ProvidersFilter Filter)
        {
            lvProviders.BeginUpdate();
            lvProviders.Items.Clear();

            bool bHasNameOrGuidFilter = !string.IsNullOrEmpty(Filter.ProviderString);
            foreach (XmlNode xnProv in Filter.Nodes.SelectNodes(string.Format("{0}", XML_PROVIDER)))
            {
                // Skip empty GUIDs
                string Guid = xnGetText(xnProv, string.Format("{0}/{1}", XML_METADATA, XML_GUID));
                if (Guid == STR_NULL_GUID)
                    continue;

                string Name = xnGetText(xnProv, XML_NAME);
                if (bHasNameOrGuidFilter &&
                     (     (Name.IndexOf(Filter.ProviderString, StringComparison.OrdinalIgnoreCase) == -1)
                        && (Guid.IndexOf(Filter.ProviderString, StringComparison.OrdinalIgnoreCase) == -1)
                     )
                   )
                {
                    continue;
                }

                var lvi = new ListViewItem(Guid);
                lvi.SubItems.Add(Name);
                lvProviders.Items.Add(lvi);

                lvi.Tag = Name;
            }

            lvProviders.EndUpdate();
        }

        private void PopulateProviderMetadata(ProviderMetadataFilter Filter)
        {
            //
            // Populate the event metadata
            //
            lvProviderMetadata.BeginUpdate();
            lvProviderMetadata.Items.Clear();

            foreach (XmlNode xnEvent in Filter.Nodes.SelectNodes(string.Format("{0}/{1}/{2}", XML_PROVIDER, XML_EVENT_METADATA, XML_EVENT)))
            {
                string Channel = xnGetText(xnEvent, XML_CHANNEL);
                if (Filter.Channels != null && !Filter.Channels.Contains(Channel))
                    continue;

                string Level = xnGetText(xnEvent, XML_LEVEL);
                if (Filter.Levels != null && !Filter.Levels.Contains(Level))
                    continue;

                string Opcode = xnGetText(xnEvent, XML_OPCODE);
                if (Filter.Opcodes != null && !Filter.Opcodes.Contains(Opcode))
                    continue;

                string TaskName = xnGetText(xnEvent, XML_TASK);
                if (Filter.Tasks != null && !Filter.Tasks.Contains(TaskName))
                    continue;

                string Message = xnGetText(xnEvent, XML_MESSAGE);
                if (!string.IsNullOrEmpty(Filter.Message) && Message.IndexOf(Filter.Message, StringComparison.OrdinalIgnoreCase) == -1)
                    continue;

                string Template = xnGetText(xnEvent, XML_TEMPLATE);
                var FieldsArray = GetProviderTemplateFields(Template);
                if (Filter.TemplateFields != null)
                {
                    var AllFields = new HashSet<string>(FieldsArray);
                    // ALL? All the fields should be a subset
                    if (Filter.TemplateFieldCondition == 0)
                    {
                        if (!Filter.TemplateFields.IsSubsetOf(AllFields))
                            continue;
                    }
                    // ANYOF? At least one filter member belongs in AllFields
                    else if (Filter.TemplateFieldCondition == 1)
                    {
                        if (!Filter.TemplateFields.Overlaps(AllFields))
                            continue;
                    }
                }

                string Id = xnGetText(xnEvent, XML_ID);

                string Keyword = xnGetText(xnEvent, XML_KEYWORD);
                string Fields = string.Join(",", FieldsArray);

                var lvi = new ListViewItem(Id);
                lvi.Tag = xnEvent;

                lvi.SubItems.AddRange(new string[]
                {
                    Level,
                    Opcode,
                    TaskName,
                    Keyword,
                    Channel,
                    Message,
                    Fields
                });

                lvProviderMetadata.Items.Add(lvi);
            }
            lvProviderMetadata.EndUpdate();
        }

        private void GetProviderMetadataFilter(ref ProviderMetadataFilter Filter)
        {
            GetCbChkSelections(
                cbchkChannels,
                out Filter.Channels);
            GetCbChkSelections(
                cbchkTasks,
                out Filter.Tasks);
            GetCbChkSelections(
                cbchkOpcodes,
                out Filter.Opcodes);
            GetCbChkSelections(
                cbchkLevels,
                out Filter.Levels);
            GetCbChkSelections(
                cbchkTemplateFields,
                out Filter.TemplateFields);
            GetCbChkSelections(
                cbchkTemplateFields,
                out Filter.TemplateFields);

            Filter.TemplateFieldCondition = cbProviderMetadataTemplateFieldsMatchCondition.SelectedIndex;
            Filter.Message = txtProviderFilterText.Text;
        }

        public void PopulateCbChkFilters(
            PresentationControls.CheckBoxComboBox CbChk,
            XmlNodeList Nodes,
            string NodeName,
            string SecondaryNodeName = null)
        {
            CbChk.BeginUpdate();
            CbChk.CheckBoxItems.Clear();
            CbChk.Clear();

            foreach (XmlNode xnNode in Nodes)
            {
                string Str = xnGetText(xnNode, NodeName);
                if (string.IsNullOrEmpty(Str) && SecondaryNodeName != null)
                    Str = xnGetText(xnNode, SecondaryNodeName);

                if (!string.IsNullOrEmpty(Str))
                    CbChk.Items.Add(Str);
            }
            CbChk.Text = "";
            CbChk.EndUpdate();
        }

        public void PopulateCbChkTemplateFieldsFilter(
            PresentationControls.CheckBoxComboBox CbChk,
            XmlNode xnProvider)
        {
            CbChk.BeginUpdate();
            CbChk.CheckBoxItems.Clear();
            CbChk.Clear();

            var AllFields = new HashSet<string>();
            foreach (XmlNode xnTemplate in xnProvider.SelectNodes(string.Format("{0}/{1}/{2}/{3}", XML_PROVIDER, XML_EVENT_METADATA, XML_EVENT, XML_TEMPLATE)))
            {
                foreach (string Field in GetProviderTemplateFields(xnTemplate.InnerText))
                    AllFields.Add(Field);
            }

            var SortedFields = new List<string>(AllFields);
            SortedFields.Sort();
            foreach (string Field in SortedFields)
            {
                if (!string.IsNullOrEmpty(Field))
                    CbChk.Items.Add(Field);
            }
            CbChk.Text = "";
            CbChk.EndUpdate();
        }

        public void GetCbChkSelections(
            PresentationControls.CheckBoxComboBox CbChk,
            out HashSet<string> ItemsText)
        {
            var set = new HashSet<string>();
            foreach (var Item in CbChk.CheckBoxItems)
            {
                if (Item.Checked)
                    set.Add(Item.Text);
            }

            ItemsText = set.Count > 0 ? set : null;
        }

        private void PopulateSelectedProviderInfo()
        {
            if (lvProviders.SelectedItems.Count == 0)
                return;

            var lvi = lvProviders.SelectedItems[0];

            // Build new empty filter + data
            LastMetadataFilter = new ProviderMetadataFilter();

            string ProviderName = lvi.Tag as string;
            ComputeProviderMetadata(
                ProviderName,
                out LastMetadataFilter.Nodes);

            GetProviderMetadataFilter(ref LastMetadataFilter);

            // Populate without restrictions
            PopulateProviderMetadata(LastMetadataFilter);

            // Populate keywords
            PopulateProviderMetadataKeywords(LastMetadataFilter.Nodes);

            gbProviderMetadata.Text = "Provider metadata - " + ProviderName;
            gbProviderFilters.Text = "Provider filters - " + ProviderName;
        }

        private void lvProviders_DoubleClick(
            object sender,
            EventArgs e)
        {
            PopulateSelectedProviderInfo();
        }

        private void PopulateProviderMetadataKeywords(XmlNode Nodes)
        {
            lvProviderKeywords.BeginUpdate();
            lvProviderKeywords.Items.Clear();
            foreach (XmlNode xnKeyword in Nodes.SelectNodes(string.Format("{0}/{1}/{2}/{3}", XML_PROVIDER, XML_METADATA, XML_KEYWORDS, XML_KEYWORD)))
            {
                long xVal;
                string Val = xnGetText(xnKeyword, XML_VALUE);
                if (long.TryParse(Val, out xVal))
                    Val = string.Format("{0:X8}", xVal);

                string Message = xnGetText(xnKeyword, XML_MESSAGE);
                string Name = xnGetText(xnKeyword, XML_NAME);

                var lvi = new ListViewItem(Val);
                lvi.SubItems.AddRange(new string[]
                {
                    Name,
                    Message
                });
                lvProviderKeywords.Items.Add(lvi);
            }
            lvProviderKeywords.EndUpdate();
        }

        private void btnProvFilterApply_Click(
            object sender,
            EventArgs e)
        {
            if (LastMetadataFilter == null)
                PopulateSelectedProviderInfo();

            GetProviderMetadataFilter(ref LastMetadataFilter);
            PopulateProviderMetadata(LastMetadataFilter);
        }

        private void ctxmenuitemProvMetaInfo_Click(
            object sender,
            EventArgs e)
        {
            XmlNode xnEvent = lvProviderMetadata.SelectedItems[0].Tag as XmlNode;
            if (xnEvent != null)
            {
                MsgBoxInfo(
                    "--------\n" +
                    "Message:\n" +
                    "--------\n" +
                    xnGetText(xnEvent, XML_MESSAGE) + "\n" +
                    "\n" +
                    "--------\n" +
                    "Template:\n" +
                    "--------\n" +
                    xnGetText(xnEvent, XML_TEMPLATE));
            }
        }

        private void lvProviders_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                PopulateSelectedProviderInfo();
        }

        private void menuMainHelpAbout_Click(
            object sender,
            EventArgs e)
        {
            MsgBoxInfo(string.Format(
@"{0}
(c) Elias Bachaalany <lallousz-x86@yahoo.com>
", STR_TITLE));
        }

        private void menuMainFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuMainFileClearCache_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                Directory.Delete(ProvidersMetadataFolder, true);
            }
            catch (Exception)
            {
            }
            InitPathes();
        }

        private string[] GetProviderTemplateFields(string Template)
        {
            Template = Template.Trim();
            if (!string.IsNullOrEmpty(Template))
            {
                try
                {
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(Template);

                    List<string> aFields = new List<string>();
                    foreach (XmlNode xnField in xd.DocumentElement.SelectNodes("*"))
                        aFields.Add((xnField.Name == "struct" ? "s:" : "") + xnField.GetAttrValue("name"));

                    return aFields.ToArray();
                }
                catch
                {
                }
            }
            return new string[] { };
        }

        private void ctxmenuitemProvMetaCopyID_Click(
            object sender, 
            EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(string.Join(",", GetSelectedColumnValues(lvProviderMetadata, 0)));
        }

        private List<string> GetSelectedColumnValues(
            ListView LV, 
            int ColID)
        {
            var Result = new List<string>();
            foreach (ListViewItem lvi in LV.SelectedItems)
                Result.Add(lvi.SubItems[ColID].Text);

            return Result;
        }

        private void ctxmenuitemProvMetaCopyIDAsCase_Click(
            object sender, 
            EventArgs e)
        {
            var IDs = GetSelectedColumnValues(lvProviderMetadata, 0);
            for (int i = 0, c = IDs.Count; i < c; i++)
                IDs[i] = "case " + IDs[i] + ":";

            Clipboard.Clear();
            Clipboard.SetText(string.Join("\n", IDs));
        }

        private void ctxmenuProviderCopyNameOrGuid_Click(
            object sender, 
            EventArgs e)
        {
            var m = sender as ToolStripMenuItem;
            int lvCol = m == null || m.Tag.ToString() == "1" ? 1 : 0;
            Clipboard.Clear();
            Clipboard.SetText(string.Join(",", GetSelectedColumnValues(lvProviders, lvCol)));
        }

        private void txtProviderGuidNameFilter_KeyUp(
            object sender, 
            KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.Handled = true;
                lvProviders.Focus();
                if (lvProviders.Items.Count > 0)
                {
                    lvProviders.Items[0].Selected = true;
                    PopulateSelectedProviderInfo();
                }
            }
        }

        private void ctxmenuKeywordsCopyFlags_Click(
            object sender, 
            EventArgs e)
        {
            var flags = new List<string>();
            foreach (ListViewItem lvi in lvProviderKeywords.CheckedItems)
            {
                string val = lvi.SubItems[0].Text;
                string desc = lvi.SubItems[1].Text;

                flags.Add(string.Format("0x{0} /* {1} */", val, desc));
            }

            if (flags.Count > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText("var keywords = \r\n\t" + string.Join(" |\r\n\t", flags) + ";");
            }
        }
    }
}