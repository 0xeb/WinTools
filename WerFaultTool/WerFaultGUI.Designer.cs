namespace WerFaultTool
{
    partial class WerFaultGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WerFaultGUI));
            this.lblConfiguredImages = new System.Windows.Forms.Label();
            this.lbConfiguredImages = new System.Windows.Forms.ListBox();
            this.txtImageName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipCtrl = new System.Windows.Forms.ToolTip(this.components);
            this.lblDumpFolder = new System.Windows.Forms.Label();
            this.btnBrowseImageName = new System.Windows.Forms.Button();
            this.btnBrowseDumpFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDumpType = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnRemoveImage = new System.Windows.Forms.Button();
            this.btnUpdateImage = new System.Windows.Forms.Button();
            this.txtDumpFolder = new System.Windows.Forms.TextBox();
            this.txtDumpCount = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gbCustomCrashOption = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.gbCustomCrashOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConfiguredImages
            // 
            this.lblConfiguredImages.AutoSize = true;
            this.lblConfiguredImages.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConfiguredImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfiguredImages.Location = new System.Drawing.Point(2, 115);
            this.lblConfiguredImages.Name = "lblConfiguredImages";
            this.lblConfiguredImages.Size = new System.Drawing.Size(97, 13);
            this.lblConfiguredImages.TabIndex = 13;
            this.lblConfiguredImages.Text = "Configured images:";
            this.toolTipCtrl.SetToolTip(this.lblConfiguredImages, "Click to refresh the list of configured images");
            this.lblConfiguredImages.Click += new System.EventHandler(this.lblConfiguredImages_Click);
            // 
            // lbConfiguredImages
            // 
            this.lbConfiguredImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbConfiguredImages.FormattingEnabled = true;
            this.lbConfiguredImages.Location = new System.Drawing.Point(5, 131);
            this.lbConfiguredImages.MultiColumn = true;
            this.lbConfiguredImages.Name = "lbConfiguredImages";
            this.lbConfiguredImages.Size = new System.Drawing.Size(674, 69);
            this.lbConfiguredImages.TabIndex = 14;
            this.lbConfiguredImages.SelectedIndexChanged += new System.EventHandler(this.lbConfiguredImages_SelectedIndexChanged);
            // 
            // txtImageName
            // 
            this.txtImageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageName.Location = new System.Drawing.Point(82, 12);
            this.txtImageName.Name = "txtImageName";
            this.txtImageName.Size = new System.Drawing.Size(561, 20);
            this.txtImageName.TabIndex = 1;
            this.txtImageName.Text = "image.exe";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image name:";
            this.toolTipCtrl.SetToolTip(this.label1, "Image name");
            // 
            // lblDumpFolder
            // 
            this.lblDumpFolder.AutoSize = true;
            this.lblDumpFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDumpFolder.Location = new System.Drawing.Point(8, 43);
            this.lblDumpFolder.Name = "lblDumpFolder";
            this.lblDumpFolder.Size = new System.Drawing.Size(67, 13);
            this.lblDumpFolder.TabIndex = 3;
            this.lblDumpFolder.Text = "Dump folder:";
            this.toolTipCtrl.SetToolTip(this.lblDumpFolder, "Location of the dump for this image");
            // 
            // btnBrowseImageName
            // 
            this.btnBrowseImageName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseImageName.Location = new System.Drawing.Point(649, 10);
            this.btnBrowseImageName.Name = "btnBrowseImageName";
            this.btnBrowseImageName.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseImageName.TabIndex = 2;
            this.btnBrowseImageName.Tag = "";
            this.btnBrowseImageName.Text = "...";
            this.toolTipCtrl.SetToolTip(this.btnBrowseImageName, "Browse image path");
            this.btnBrowseImageName.UseVisualStyleBackColor = true;
            this.btnBrowseImageName.Click += new System.EventHandler(this.btnBrowseImageName_Click);
            // 
            // btnBrowseDumpFolder
            // 
            this.btnBrowseDumpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDumpFolder.Location = new System.Drawing.Point(649, 38);
            this.btnBrowseDumpFolder.Name = "btnBrowseDumpFolder";
            this.btnBrowseDumpFolder.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDumpFolder.TabIndex = 5;
            this.btnBrowseDumpFolder.Tag = "";
            this.btnBrowseDumpFolder.Text = "...";
            this.toolTipCtrl.SetToolTip(this.btnBrowseDumpFolder, "Browse dump folder");
            this.btnBrowseDumpFolder.UseVisualStyleBackColor = true;
            this.btnBrowseDumpFolder.Click += new System.EventHandler(this.btnBrowseDumpFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Location = new System.Drawing.Point(8, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Dump count:";
            this.toolTipCtrl.SetToolTip(this.label4, "Dump counts to preserve");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Location = new System.Drawing.Point(8, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Dump type:";
            this.toolTipCtrl.SetToolTip(this.label3, "Dump counts to preserve");
            // 
            // cbDumpType
            // 
            this.cbDumpType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDumpType.FormattingEnabled = true;
            this.cbDumpType.Items.AddRange(new object[] {
            "Custom dump",
            "Mini dump",
            "Full dump"});
            this.cbDumpType.Location = new System.Drawing.Point(82, 63);
            this.cbDumpType.Name = "cbDumpType";
            this.cbDumpType.Size = new System.Drawing.Size(162, 21);
            this.cbDumpType.TabIndex = 7;
            this.toolTipCtrl.SetToolTip(this.cbDumpType, "Dump type");
            this.cbDumpType.SelectedIndexChanged += new System.EventHandler(this.cbDumpType_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Image = global::WerFaultTool.Properties.Resources.about;
            this.button3.Location = new System.Drawing.Point(609, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(34, 34);
            this.button3.TabIndex = 10;
            this.toolTipCtrl.SetToolTip(this.button3, "About and Help");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveImage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveImage.Image = global::WerFaultTool.Properties.Resources.minus;
            this.btnRemoveImage.Location = new System.Drawing.Point(569, 64);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(34, 34);
            this.btnRemoveImage.TabIndex = 9;
            this.toolTipCtrl.SetToolTip(this.btnRemoveImage, "Delete selected entry");
            this.btnRemoveImage.UseVisualStyleBackColor = true;
            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);
            // 
            // btnUpdateImage
            // 
            this.btnUpdateImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateImage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateImage.Image = global::WerFaultTool.Properties.Resources._checked;
            this.btnUpdateImage.Location = new System.Drawing.Point(529, 64);
            this.btnUpdateImage.Name = "btnUpdateImage";
            this.btnUpdateImage.Size = new System.Drawing.Size(34, 34);
            this.btnUpdateImage.TabIndex = 8;
            this.toolTipCtrl.SetToolTip(this.btnUpdateImage, "Update");
            this.btnUpdateImage.UseVisualStyleBackColor = true;
            this.btnUpdateImage.Click += new System.EventHandler(this.btnUpdateImage_Click);
            // 
            // txtDumpFolder
            // 
            this.txtDumpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDumpFolder.Location = new System.Drawing.Point(82, 38);
            this.txtDumpFolder.Name = "txtDumpFolder";
            this.txtDumpFolder.Size = new System.Drawing.Size(561, 20);
            this.txtDumpFolder.TabIndex = 4;
            this.txtDumpFolder.Text = "C:\\Temp";
            // 
            // txtDumpCount
            // 
            this.txtDumpCount.Location = new System.Drawing.Point(82, 88);
            this.txtDumpCount.Name = "txtDumpCount";
            this.txtDumpCount.Size = new System.Drawing.Size(54, 20);
            this.txtDumpCount.TabIndex = 12;
            this.txtDumpCount.Text = "10";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Executable files|*.exe|All files|*.*";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // gbCustomCrashOption
            // 
            this.gbCustomCrashOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCustomCrashOption.Controls.Add(this.flowLayoutPanelOptions);
            this.gbCustomCrashOption.Location = new System.Drawing.Point(5, 206);
            this.gbCustomCrashOption.Name = "gbCustomCrashOption";
            this.gbCustomCrashOption.Size = new System.Drawing.Size(674, 262);
            this.gbCustomCrashOption.TabIndex = 17;
            this.gbCustomCrashOption.TabStop = false;
            this.gbCustomCrashOption.Text = "Custom crash options";
            // 
            // flowLayoutPanelOptions
            // 
            this.flowLayoutPanelOptions.AutoScroll = true;
            this.flowLayoutPanelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelOptions.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanelOptions.Name = "flowLayoutPanelOptions";
            this.flowLayoutPanelOptions.Size = new System.Drawing.Size(668, 243);
            this.flowLayoutPanelOptions.TabIndex = 18;
            // 
            // WerFaultGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 471);
            this.Controls.Add(this.gbCustomCrashOption);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDumpType);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtDumpCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowseDumpFolder);
            this.Controls.Add(this.btnBrowseImageName);
            this.Controls.Add(this.txtDumpFolder);
            this.Controls.Add(this.lblDumpFolder);
            this.Controls.Add(this.btnRemoveImage);
            this.Controls.Add(this.btnUpdateImage);
            this.Controls.Add(this.lblConfiguredImages);
            this.Controls.Add(this.lbConfiguredImages);
            this.Controls.Add(this.txtImageName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(697, 495);
            this.Name = "WerFaultGUI";
            this.Text = "WerFault Tool - v1.0.0 (c) Elias Bachaalany";
            this.gbCustomCrashOption.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblConfiguredImages;
        private System.Windows.Forms.ListBox lbConfiguredImages;
        private System.Windows.Forms.TextBox txtImageName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdateImage;
        private System.Windows.Forms.Button btnRemoveImage;
        private System.Windows.Forms.ToolTip toolTipCtrl;
        private System.Windows.Forms.TextBox txtDumpFolder;
        private System.Windows.Forms.Label lblDumpFolder;
        private System.Windows.Forms.Button btnBrowseImageName;
        private System.Windows.Forms.Button btnBrowseDumpFolder;
        private System.Windows.Forms.TextBox txtDumpCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDumpType;
        private System.Windows.Forms.GroupBox gbCustomCrashOption;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOptions;
    }
}

