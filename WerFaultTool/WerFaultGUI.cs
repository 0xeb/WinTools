using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WerFaultTool
{
    public partial class WerFaultGUI : Form
    {
        const string REGPATH_WER = @"SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps";
        const string REGVAL_DUMPFOLDER = "DumpFolder";
        const string REGVAL_DUMPCOUNT = "DumpCount";
        const string REGVAL_DUMPTYPE = "DumpType";
        const string REGVAL_CUSTOMDUMPFLAGS = "CustomDumpFlags";

        public CustomFlagsDef[] opts = new CustomFlagsDef[]
        {
            new CustomFlagsDef { Name = "Dump normal", Flag = _MINIDUMP_TYPE.MiniDumpNormal, Description = @"Include just the information necessary to capture stack traces for all existing threads in a process." },
            new CustomFlagsDef { Name = "Dump with full memory", Flag = _MINIDUMP_TYPE.MiniDumpWithFullMemory, Description = @"Include all accessible memory in the process. The raw memory data is included at the end, so that the initial structures can be mapped directly without the raw memory information. This option can result in a very large file." },
            new CustomFlagsDef { Name = "Dump with data segments", Flag = _MINIDUMP_TYPE.MiniDumpWithDataSegs, Description = @"Include the data sections from all loaded modules. This results in the inclusion of global variables, which can make the minidump file significantly larger. For per-module control, use the ModuleWriteDataSeg enumeration value from MODULE_WRITE_FLAGS." },
            new CustomFlagsDef { Name = "Dump with handle data", Flag = _MINIDUMP_TYPE.MiniDumpWithHandleData, Description = @"Include high-level information about the operating system handles that are active when the minidump is made." },
            new CustomFlagsDef { Name = "Filter memory", Flag = _MINIDUMP_TYPE.MiniDumpFilterMemory, Description = @"Stack and backing store memory written to the minidump file should be filtered to remove all but the pointer values necessary to reconstruct a stack trace." },
            new CustomFlagsDef { Name = "Scan memory", Flag = _MINIDUMP_TYPE.MiniDumpScanMemory, Description = @"Stack and backing store memory should be scanned for pointer references to modules in the module list. If a module is referenced by stack or backing store memory, the ModuleWriteFlags member of the MINIDUMP_CALLBACK_OUTPUT structure is set to ModuleReferencedByMemory." },
            new CustomFlagsDef { Name = "Include unloaded modules", Flag = _MINIDUMP_TYPE.MiniDumpWithUnloadedModules, Description = @"Include information from the list of modules that were recently unloaded, if this information is maintained by the operating system." },
            new CustomFlagsDef { Name = "Include data references", Flag = _MINIDUMP_TYPE.MiniDumpWithIndirectlyReferencedMemory, Description = @"Include pages with data referenced by locals or other stack memory. This option can increase the size of the minidump file significantly." },
            new CustomFlagsDef { Name = "Filter module path info", Flag = _MINIDUMP_TYPE.MiniDumpFilterModulePaths, Description = @"Filter module paths for information such as user names or important directories.This option may prevent the system from locating the image file and should be used only in special situations." },
            new CustomFlagsDef { Name = "Include process and thread information", Flag = _MINIDUMP_TYPE.MiniDumpWithProcessThreadData, Description = @"Include complete per-process and per-thread information from the operating system." },
            new CustomFlagsDef { Name = "Include PAGE_READWRITE memory pages", Flag = _MINIDUMP_TYPE.MiniDumpWithPrivateReadWriteMemory, Description = @"Scan the virtual address space for PAGE_READWRITE memory to be included." },
            new CustomFlagsDef { Name = "Dump without optional data", Flag = _MINIDUMP_TYPE.MiniDumpWithoutOptionalData, Description = @"Reduce the data that is dumped by eliminating memory regions that are not essential to meet criteria specified for the dump. This can avoid dumping memory that may contain data that is private to the user. However, it is not a guarantee that no private information will be present." },
            new CustomFlagsDef { Name = "Include memory region information", Flag = _MINIDUMP_TYPE.MiniDumpWithFullMemoryInfo, Description = @"Include memory region information.For more information, see MINIDUMP_MEMORY_INFO_LIST." },
            new CustomFlagsDef { Name = "Include thread state information", Flag = _MINIDUMP_TYPE.MiniDumpWithThreadInfo, Description = @"Include thread state information. For more information, see MINIDUMP_THREAD_INFO_LIST." },
            new CustomFlagsDef { Name = "Include all code-related segments", Flag = _MINIDUMP_TYPE.MiniDumpWithCodeSegs, Description = @"Include all code and code-related sections from loaded modules to capture executable content. For per-module control, use the ModuleWriteCodeSegs enumeration value from MODULE_WRITE_FLAGS." },
            new CustomFlagsDef { Name = "Exclude auxiliary state", Flag = _MINIDUMP_TYPE.MiniDumpWithoutAuxiliaryState, Description = @"Turns off secondary auxiliary-supported memory gathering." },
            new CustomFlagsDef { Name = "Include auxiliary state", Flag = _MINIDUMP_TYPE.MiniDumpWithFullAuxiliaryState, Description = @"Requests that auxiliary data providers include their state in the dump image; the state data that is included is provider dependent.This option can result in a large dump image." },
            new CustomFlagsDef { Name = "Include PAGE_WRITECOPY memory pages", Flag = _MINIDUMP_TYPE.MiniDumpWithPrivateWriteCopyMemory, Description = @"Scans the virtual address space for PAGE_WRITECOPY memory to be included." },
            new CustomFlagsDef { Name = "Don't fail on inaccessible memory info", Flag = _MINIDUMP_TYPE.MiniDumpIgnoreInaccessibleMemory, Description = @"If you specify MiniDumpWithFullMemory, the MiniDumpWriteDump function will fail if the function cannot read the memory regions; however, if you include MiniDumpIgnoreInaccessibleMemory, the MiniDumpWriteDump function will ignore the memory read failures and continue to generate the dump.Note that the inaccessible memory regions are not included in the dump." },
            new CustomFlagsDef { Name = "Include security token information", Flag = _MINIDUMP_TYPE.MiniDumpWithTokenInformation, Description = @"Adds security token related data. This will make the ""!token"" extension work when processing a user-mode dump." },
            new CustomFlagsDef { Name = "Include module header information", Flag = _MINIDUMP_TYPE.MiniDumpWithModuleHeaders, Description = @"Adds module header related data." },
            new CustomFlagsDef { Name = "Add filter triage information", Flag = _MINIDUMP_TYPE.MiniDumpFilterTriage, Description = @"Adds filter triage related data." },
        };

        public WerFaultGUI()
        {
            InitializeComponent();
            CreateDyanmicControls();
            cbDumpType.SelectedIndex = 0;
            PopulateWerFaultImages();
        }

        /// <summary>
        /// Create the dynamic options controls
        /// </summary>
        private void CreateDyanmicControls()
        {
            // Find out the largest text
            int largest_name = -1;
            int largest_idx = 0;
            for (int i=0, c = opts.Length; i< c; ++i)
            {
                if (opts[i].Name.Length > largest_name)
                {
                    largest_name = opts[i].Name.Length;
                    largest_idx = i;
                }
            }
            Size textSize = TextRenderer.MeasureText(opts[largest_idx].Name + "WW", Font);

            foreach (var opt in opts)
            {
                var chk = new CheckBox() { Text = opt.Name, Width = textSize.Width };
                chk.Tag = opt;
                chk.Cursor = Cursors.Hand;
                toolTipCtrl.SetToolTip(chk, opt.Description);
                flowLayoutPanelOptions.Controls.Add(chk);
            }
        }

        private void PopulateWerFaultImages()
        {
            lbConfiguredImages.BeginUpdate();
            lbConfiguredImages.Items.Clear();

            try
            {
                using (var reg = Registry.LocalMachine.OpenSubKey(REGPATH_WER))
                {
                    foreach (var ImageName in reg.GetSubKeyNames())
                        lbConfiguredImages.Items.Add(ImageName);
                }
            }
            catch
            {
            }

            lbConfiguredImages.EndUpdate();
            if (lbConfiguredImages.Items.Count > 0)
                lbConfiguredImages.SelectedIndex = 0;
            else
                (flowLayoutPanelOptions.Controls[0] as CheckBox).Checked = true;
        }

        /// <summary>
        /// Update the UI from a given entry
        /// </summary>
        private void UpdateEntryUI(WerFaultEntry Entry)
        {
            txtDumpCount.Text  = Entry.DumpCount.ToString();
            txtImageName.Text  = Entry.ImageName;
            txtDumpFolder.Text = Entry.DumpFolder;
            cbDumpType.SelectedIndex = Math.Max(0, Math.Min(2, Entry.DumpType));

            // Update the custom dump flags
            foreach (var ctrl in flowLayoutPanelOptions.Controls)
            {
                var chk = ctrl as CheckBox;
                var opt = chk.Tag as CustomFlagsDef;
                chk.Checked = (Entry.CustomDumpFlags & (uint)opt.Flag) != 0;
            }
            if (Entry.CustomDumpFlags == 0)
                (flowLayoutPanelOptions.Controls[0] as CheckBox).Checked = true;
        }

        private void lblConfiguredImages_Click(object sender, EventArgs e)
        {
            PopulateWerFaultImages();
        }

        /// <summary>
        /// Load a WerFault entry by name
        /// </summary>
        private WerFaultEntry LoadWerFaultEntry(string ImageName)
        {
            try
            {
                using (var reg = Registry.LocalMachine.OpenSubKey(Path.Combine(REGPATH_WER, ImageName)))
                {
                    var Entry = new WerFaultEntry()
                    {
                        ImageName = ImageName,
                        DumpType = (int)reg.GetValue(REGVAL_DUMPTYPE, 0),
                        CustomDumpFlags = (uint)(int)reg.GetValue(REGVAL_CUSTOMDUMPFLAGS, 0),
                        DumpCount = (int)reg.GetValue(REGVAL_DUMPCOUNT, 0),
                        DumpFolder = (string)reg.GetValue(REGVAL_DUMPFOLDER, "")
                    };
                    return Entry;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Update the UI when the selection changes
        /// </summary>
        private void lbConfiguredImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lb = (sender as ListBox);
            var idx = lb.SelectedIndex;
            if (idx != -1)
                UpdateEntryUI(LoadWerFaultEntry(lb.Items[idx] as string));
        }

        private void btnBrowseDumpFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtDumpFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnBrowseImageName_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtImageName.Text = Path.GetFileName(openFileDialog1.FileName);
        }

        private void btnUpdateImage_Click(object sender, EventArgs e)
        {
            var Entry = new WerFaultEntry()
            {
                ImageName = Path.GetFileName(txtImageName.Text),
                DumpFolder = txtDumpFolder.Text,
                DumpType = cbDumpType.SelectedIndex
            };

            if (string.IsNullOrEmpty(Entry.ImageName))
            {
                MessageBox.Show("Please select an image name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtImageName.Focus();
                return;
            }

            if (!Directory.Exists(Entry.DumpFolder))
            {
                MessageBox.Show("Dump folder " + Entry.DumpFolder + " does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDumpFolder.Focus();
                return;
            }
            if (!int.TryParse(txtDumpCount.Text, out Entry.DumpCount))
            {
                MessageBox.Show("Dump count invalid. Specify a number > 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDumpCount.Focus();
                return;
            }

            foreach (var ctrl in flowLayoutPanelOptions.Controls)
            {
                var chk = ctrl as CheckBox;
                var opt = chk.Tag as CustomFlagsDef;
                if (chk.Checked)
                    Entry.CustomDumpFlags |= (uint)opt.Flag;
            }

            try
            {
                bool bNew;
                var regPath = Path.Combine(REGPATH_WER, Entry.ImageName);
                var reg = Registry.LocalMachine.OpenSubKey(regPath);
                if (reg == null)
                {
                    bNew = true;
                }
                else
                {
                    reg.Close();
                    bNew = false;
                }

                using (reg = Registry.LocalMachine.CreateSubKey(regPath))
                {
                    reg.SetValue(REGVAL_DUMPFOLDER, Entry.DumpFolder, RegistryValueKind.String);
                    reg.SetValue(REGVAL_DUMPTYPE, Entry.DumpType, RegistryValueKind.DWord);
                    reg.SetValue(REGVAL_CUSTOMDUMPFLAGS, Entry.CustomDumpFlags, RegistryValueKind.DWord);
                    reg.SetValue(REGVAL_DUMPCOUNT, Entry.DumpCount, RegistryValueKind.DWord);

                }
                if (bNew)
                {
                    lbConfiguredImages.Items.Add(Entry.ImageName);
                    lbConfiguredImages.SelectedIndex = lbConfiguredImages.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            var idx = lbConfiguredImages.SelectedIndex;
            if (idx == -1)
                return;

            var ImageName = (lbConfiguredImages.Items[idx]) as string;
            try
            {
                Registry.LocalMachine.DeleteSubKey(Path.Combine(REGPATH_WER, ImageName));

                // Remove visually
                lbConfiguredImages.Items.RemoveAt(idx);
                if (idx >= 1)
                    lbConfiguredImages.SelectedIndex = idx - 1;
            }
            catch
            {
            }
        }

        private void cbDumpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (sender as ComboBox);
            gbCustomCrashOption.Enabled = cb.SelectedIndex == 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new AboutForm()).ShowDialog();
        }
    }
}
