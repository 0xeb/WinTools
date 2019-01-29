namespace Explore
{
    partial class WEPExplorer
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
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties3 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties4 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties5 = new PresentationControls.CheckBoxProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WEPExplorer));
            this.gbProviderFilters = new System.Windows.Forms.GroupBox();
            this.cbProviderMetadataTemplateFieldsMatchCondition = new System.Windows.Forms.ComboBox();
            this.cbchkTemplateFields = new PresentationControls.CheckBoxComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbchkTasks = new PresentationControls.CheckBoxComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnProvFilterApply = new System.Windows.Forms.Button();
            this.txtProviderFilterText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbchkOpcodes = new PresentationControls.CheckBoxComboBox();
            this.cbchkLevels = new PresentationControls.CheckBoxComboBox();
            this.cbchkChannels = new PresentationControls.CheckBoxComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbProviderKeywords = new System.Windows.Forms.GroupBox();
            this.lvProviderKeywords = new System.Windows.Forms.ListView();
            this.lvcKeywordsValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcKeywordsName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcKeywordsMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtProviderNameGuidFilter = new System.Windows.Forms.TextBox();
            this.lblProviderNameGuid = new System.Windows.Forms.Label();
            this.lvProviders = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbProviderMetadata = new System.Windows.Forms.GroupBox();
            this.lvProviderMetadata = new System.Windows.Forms.ListView();
            this.lvcProvMetadataId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataOpcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataTask = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataKeyword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcProvMetadataFields = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctxmenuProvMetadata = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxmenuitemProvMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxmenuitemProvMetaCopyID = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxmenuitemProvMetaCopyIDAsCase = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxmenuProviders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxmenuProviderCopyName = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxmenuProviderCopyGuid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileClearCache = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxmenuProvKeywords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxmenuKeywordsCopyFlags = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.gbProviderFilters.SuspendLayout();
            this.gbProviderKeywords.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbProviderMetadata.SuspendLayout();
            this.ctxmenuProvMetadata.SuspendLayout();
            this.ctxmenuProviders.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.ctxmenuProvKeywords.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(132, 6);
            // 
            // gbProviderFilters
            // 
            this.gbProviderFilters.Controls.Add(this.cbProviderMetadataTemplateFieldsMatchCondition);
            this.gbProviderFilters.Controls.Add(this.cbchkTemplateFields);
            this.gbProviderFilters.Controls.Add(this.label7);
            this.gbProviderFilters.Controls.Add(this.cbchkTasks);
            this.gbProviderFilters.Controls.Add(this.label6);
            this.gbProviderFilters.Controls.Add(this.btnProvFilterApply);
            this.gbProviderFilters.Controls.Add(this.txtProviderFilterText);
            this.gbProviderFilters.Controls.Add(this.label5);
            this.gbProviderFilters.Controls.Add(this.cbchkOpcodes);
            this.gbProviderFilters.Controls.Add(this.cbchkLevels);
            this.gbProviderFilters.Controls.Add(this.cbchkChannels);
            this.gbProviderFilters.Controls.Add(this.label3);
            this.gbProviderFilters.Controls.Add(this.label2);
            this.gbProviderFilters.Controls.Add(this.label1);
            this.gbProviderFilters.Location = new System.Drawing.Point(7, 209);
            this.gbProviderFilters.Name = "gbProviderFilters";
            this.gbProviderFilters.Size = new System.Drawing.Size(460, 225);
            this.gbProviderFilters.TabIndex = 2;
            this.gbProviderFilters.TabStop = false;
            this.gbProviderFilters.Text = "Provider filters";
            // 
            // cbProviderMetadataTemplateFieldsMatchCondition
            // 
            this.cbProviderMetadataTemplateFieldsMatchCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProviderMetadataTemplateFieldsMatchCondition.FormattingEnabled = true;
            this.cbProviderMetadataTemplateFieldsMatchCondition.Items.AddRange(new object[] {
            "ALL",
            "ANYOF"});
            this.cbProviderMetadataTemplateFieldsMatchCondition.Location = new System.Drawing.Point(350, 135);
            this.cbProviderMetadataTemplateFieldsMatchCondition.Name = "cbProviderMetadataTemplateFieldsMatchCondition";
            this.cbProviderMetadataTemplateFieldsMatchCondition.Size = new System.Drawing.Size(104, 21);
            this.cbProviderMetadataTemplateFieldsMatchCondition.TabIndex = 10;
            // 
            // cbchkTemplateFields
            // 
            this.cbchkTemplateFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbchkTemplateFields.CheckBoxProperties = checkBoxProperties1;
            this.cbchkTemplateFields.DisplayMemberSingleItem = "";
            this.cbchkTemplateFields.FormattingEnabled = true;
            this.cbchkTemplateFields.Location = new System.Drawing.Point(83, 135);
            this.cbchkTemplateFields.Name = "cbchkTemplateFields";
            this.cbchkTemplateFields.Size = new System.Drawing.Size(261, 21);
            this.cbchkTemplateFields.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Template fields:";
            // 
            // cbchkTasks
            // 
            this.cbchkTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbchkTasks.CheckBoxProperties = checkBoxProperties2;
            this.cbchkTasks.DisplayMemberSingleItem = "";
            this.cbchkTasks.FormattingEnabled = true;
            this.cbchkTasks.Location = new System.Drawing.Point(83, 106);
            this.cbchkTasks.Name = "cbchkTasks";
            this.cbchkTasks.Size = new System.Drawing.Size(371, 21);
            this.cbchkTasks.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Tasks:";
            // 
            // btnProvFilterApply
            // 
            this.btnProvFilterApply.Location = new System.Drawing.Point(83, 192);
            this.btnProvFilterApply.Name = "btnProvFilterApply";
            this.btnProvFilterApply.Size = new System.Drawing.Size(129, 23);
            this.btnProvFilterApply.TabIndex = 13;
            this.btnProvFilterApply.Text = "&Apply";
            this.btnProvFilterApply.UseVisualStyleBackColor = true;
            this.btnProvFilterApply.Click += new System.EventHandler(this.btnProvFilterApply_Click);
            // 
            // txtProviderFilterText
            // 
            this.txtProviderFilterText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProviderFilterText.Location = new System.Drawing.Point(83, 163);
            this.txtProviderFilterText.Name = "txtProviderFilterText";
            this.txtProviderFilterText.Size = new System.Drawing.Size(371, 20);
            this.txtProviderFilterText.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Message has:";
            // 
            // cbchkOpcodes
            // 
            this.cbchkOpcodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbchkOpcodes.CheckBoxProperties = checkBoxProperties3;
            this.cbchkOpcodes.DisplayMemberSingleItem = "";
            this.cbchkOpcodes.FormattingEnabled = true;
            this.cbchkOpcodes.Location = new System.Drawing.Point(83, 79);
            this.cbchkOpcodes.Name = "cbchkOpcodes";
            this.cbchkOpcodes.Size = new System.Drawing.Size(371, 21);
            this.cbchkOpcodes.TabIndex = 5;
            // 
            // cbchkLevels
            // 
            this.cbchkLevels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbchkLevels.CheckBoxProperties = checkBoxProperties4;
            this.cbchkLevels.DisplayMemberSingleItem = "";
            this.cbchkLevels.FormattingEnabled = true;
            this.cbchkLevels.Location = new System.Drawing.Point(83, 52);
            this.cbchkLevels.Name = "cbchkLevels";
            this.cbchkLevels.Size = new System.Drawing.Size(371, 21);
            this.cbchkLevels.TabIndex = 3;
            // 
            // cbchkChannels
            // 
            this.cbchkChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbchkChannels.CheckBoxProperties = checkBoxProperties5;
            this.cbchkChannels.DisplayMemberSingleItem = "";
            this.cbchkChannels.FormattingEnabled = true;
            this.cbchkChannels.Location = new System.Drawing.Point(83, 23);
            this.cbchkChannels.Name = "cbchkChannels";
            this.cbchkChannels.Size = new System.Drawing.Size(371, 21);
            this.cbchkChannels.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Opcodes:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Levels:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Channels:";
            // 
            // gbProviderKeywords
            // 
            this.gbProviderKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProviderKeywords.Controls.Add(this.lvProviderKeywords);
            this.gbProviderKeywords.Location = new System.Drawing.Point(470, 209);
            this.gbProviderKeywords.Name = "gbProviderKeywords";
            this.gbProviderKeywords.Size = new System.Drawing.Size(390, 225);
            this.gbProviderKeywords.TabIndex = 3;
            this.gbProviderKeywords.TabStop = false;
            this.gbProviderKeywords.Text = "Keywords";
            // 
            // lvProviderKeywords
            // 
            this.lvProviderKeywords.CheckBoxes = true;
            this.lvProviderKeywords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcKeywordsValue,
            this.lvcKeywordsName,
            this.lvcKeywordsMessage});
            this.lvProviderKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProviderKeywords.FullRowSelect = true;
            this.lvProviderKeywords.Location = new System.Drawing.Point(3, 16);
            this.lvProviderKeywords.MultiSelect = false;
            this.lvProviderKeywords.Name = "lvProviderKeywords";
            this.lvProviderKeywords.Size = new System.Drawing.Size(384, 206);
            this.lvProviderKeywords.TabIndex = 0;
            this.lvProviderKeywords.UseCompatibleStateImageBehavior = false;
            this.lvProviderKeywords.View = System.Windows.Forms.View.Details;
            // 
            // lvcKeywordsValue
            // 
            this.lvcKeywordsValue.Text = "Value";
            this.lvcKeywordsValue.Width = 100;
            // 
            // lvcKeywordsName
            // 
            this.lvcKeywordsName.Text = "Name";
            this.lvcKeywordsName.Width = 134;
            // 
            // lvcKeywordsMessage
            // 
            this.lvcKeywordsMessage.Text = "Message";
            this.lvcKeywordsMessage.Width = 148;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtProviderNameGuidFilter);
            this.groupBox3.Controls.Add(this.lblProviderNameGuid);
            this.groupBox3.Controls.Add(this.lvProviders);
            this.groupBox3.Location = new System.Drawing.Point(7, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(859, 191);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Providers";
            // 
            // txtProviderNameGuidFilter
            // 
            this.txtProviderNameGuidFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProviderNameGuidFilter.Location = new System.Drawing.Point(116, 20);
            this.txtProviderNameGuidFilter.Name = "txtProviderNameGuidFilter";
            this.txtProviderNameGuidFilter.Size = new System.Drawing.Size(737, 20);
            this.txtProviderNameGuidFilter.TabIndex = 1;
            this.txtProviderNameGuidFilter.TextChanged += new System.EventHandler(this.txtProviderNameGuidFilter_TextChanged);
            this.txtProviderNameGuidFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtProviderGuidNameFilter_KeyUp);
            // 
            // lblProviderNameGuid
            // 
            this.lblProviderNameGuid.AutoSize = true;
            this.lblProviderNameGuid.Location = new System.Drawing.Point(3, 22);
            this.lblProviderNameGuid.Name = "lblProviderNameGuid";
            this.lblProviderNameGuid.Size = new System.Drawing.Size(110, 13);
            this.lblProviderNameGuid.TabIndex = 0;
            this.lblProviderNameGuid.Text = "&Provider GUID/name:";
            // 
            // lvProviders
            // 
            this.lvProviders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvProviders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvProviders.FullRowSelect = true;
            this.lvProviders.Location = new System.Drawing.Point(3, 45);
            this.lvProviders.Name = "lvProviders";
            this.lvProviders.Size = new System.Drawing.Size(850, 140);
            this.lvProviders.TabIndex = 2;
            this.lvProviders.UseCompatibleStateImageBehavior = false;
            this.lvProviders.View = System.Windows.Forms.View.Details;
            this.lvProviders.DoubleClick += new System.EventHandler(this.lvProviders_DoubleClick);
            this.lvProviders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvProviders_KeyPress);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "GUID";
            this.columnHeader1.Width = 279;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 381;
            // 
            // gbProviderMetadata
            // 
            this.gbProviderMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProviderMetadata.Controls.Add(this.lvProviderMetadata);
            this.gbProviderMetadata.Location = new System.Drawing.Point(7, 440);
            this.gbProviderMetadata.Name = "gbProviderMetadata";
            this.gbProviderMetadata.Size = new System.Drawing.Size(853, 301);
            this.gbProviderMetadata.TabIndex = 4;
            this.gbProviderMetadata.TabStop = false;
            this.gbProviderMetadata.Text = "Provider metadata";
            // 
            // lvProviderMetadata
            // 
            this.lvProviderMetadata.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcProvMetadataId,
            this.lvcProvMetadataLevel,
            this.lvcProvMetadataOpcode,
            this.lvcProvMetadataTask,
            this.lvcProvMetadataKeyword,
            this.lvcProvMetadataChannel,
            this.lvcProvMetadataMessage,
            this.lvcProvMetadataFields});
            this.lvProviderMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProviderMetadata.FullRowSelect = true;
            this.lvProviderMetadata.Location = new System.Drawing.Point(3, 16);
            this.lvProviderMetadata.Name = "lvProviderMetadata";
            this.lvProviderMetadata.Size = new System.Drawing.Size(847, 282);
            this.lvProviderMetadata.TabIndex = 0;
            this.lvProviderMetadata.UseCompatibleStateImageBehavior = false;
            this.lvProviderMetadata.View = System.Windows.Forms.View.Details;
            // 
            // lvcProvMetadataId
            // 
            this.lvcProvMetadataId.Text = "Id";
            // 
            // lvcProvMetadataLevel
            // 
            this.lvcProvMetadataLevel.Text = "Level";
            this.lvcProvMetadataLevel.Width = 106;
            // 
            // lvcProvMetadataOpcode
            // 
            this.lvcProvMetadataOpcode.Text = "Opcode";
            this.lvcProvMetadataOpcode.Width = 115;
            // 
            // lvcProvMetadataTask
            // 
            this.lvcProvMetadataTask.Text = "Task";
            this.lvcProvMetadataTask.Width = 164;
            // 
            // lvcProvMetadataKeyword
            // 
            this.lvcProvMetadataKeyword.Text = "Keyword";
            // 
            // lvcProvMetadataChannel
            // 
            this.lvcProvMetadataChannel.Text = "Channel";
            this.lvcProvMetadataChannel.Width = 205;
            // 
            // lvcProvMetadataMessage
            // 
            this.lvcProvMetadataMessage.Text = "Message";
            this.lvcProvMetadataMessage.Width = 300;
            // 
            // lvcProvMetadataFields
            // 
            this.lvcProvMetadataFields.Text = "Fields";
            // 
            // ctxmenuProvMetadata
            // 
            this.ctxmenuProvMetadata.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxmenuitemProvMetaInfo,
            this.toolStripMenuItem2,
            this.ctxmenuitemProvMetaCopyID,
            this.ctxmenuitemProvMetaCopyIDAsCase});
            this.ctxmenuProvMetadata.Name = "ctxmenuProvMetadata";
            this.ctxmenuProvMetadata.Size = new System.Drawing.Size(175, 76);
            // 
            // ctxmenuitemProvMetaInfo
            // 
            this.ctxmenuitemProvMetaInfo.Name = "ctxmenuitemProvMetaInfo";
            this.ctxmenuitemProvMetaInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.ctxmenuitemProvMetaInfo.Size = new System.Drawing.Size(174, 22);
            this.ctxmenuitemProvMetaInfo.Text = "Information";
            this.ctxmenuitemProvMetaInfo.Click += new System.EventHandler(this.ctxmenuitemProvMetaInfo_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(171, 6);
            // 
            // ctxmenuitemProvMetaCopyID
            // 
            this.ctxmenuitemProvMetaCopyID.Name = "ctxmenuitemProvMetaCopyID";
            this.ctxmenuitemProvMetaCopyID.Size = new System.Drawing.Size(174, 22);
            this.ctxmenuitemProvMetaCopyID.Text = "Copy IDs";
            this.ctxmenuitemProvMetaCopyID.Click += new System.EventHandler(this.ctxmenuitemProvMetaCopyID_Click);
            // 
            // ctxmenuitemProvMetaCopyIDAsCase
            // 
            this.ctxmenuitemProvMetaCopyIDAsCase.Name = "ctxmenuitemProvMetaCopyIDAsCase";
            this.ctxmenuitemProvMetaCopyIDAsCase.Size = new System.Drawing.Size(174, 22);
            this.ctxmenuitemProvMetaCopyIDAsCase.Text = "Copy IDs as \"case\"";
            this.ctxmenuitemProvMetaCopyIDAsCase.Click += new System.EventHandler(this.ctxmenuitemProvMetaCopyIDAsCase_Click);
            // 
            // ctxmenuProviders
            // 
            this.ctxmenuProviders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxmenuProviderCopyName,
            this.ctxmenuProviderCopyGuid});
            this.ctxmenuProviders.Name = "ctxmenuProvMetadata";
            this.ctxmenuProviders.Size = new System.Drawing.Size(183, 48);
            // 
            // ctxmenuProviderCopyName
            // 
            this.ctxmenuProviderCopyName.Name = "ctxmenuProviderCopyName";
            this.ctxmenuProviderCopyName.Size = new System.Drawing.Size(182, 22);
            this.ctxmenuProviderCopyName.Tag = "1";
            this.ctxmenuProviderCopyName.Text = "Copy provider name";
            this.ctxmenuProviderCopyName.Click += new System.EventHandler(this.ctxmenuProviderCopyNameOrGuid_Click);
            // 
            // ctxmenuProviderCopyGuid
            // 
            this.ctxmenuProviderCopyGuid.Name = "ctxmenuProviderCopyGuid";
            this.ctxmenuProviderCopyGuid.Size = new System.Drawing.Size(182, 22);
            this.ctxmenuProviderCopyGuid.Tag = "2";
            this.ctxmenuProviderCopyGuid.Text = "Copy provider GUID";
            this.ctxmenuProviderCopyGuid.Click += new System.EventHandler(this.ctxmenuProviderCopyNameOrGuid_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(865, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileClearCache,
            toolStripMenuItem1,
            this.menuMainFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuMainFileClearCache
            // 
            this.menuMainFileClearCache.Name = "menuMainFileClearCache";
            this.menuMainFileClearCache.Size = new System.Drawing.Size(135, 22);
            this.menuMainFileClearCache.Text = "Clear cache";
            this.menuMainFileClearCache.Click += new System.EventHandler(this.menuMainFileClearCache_Click);
            // 
            // menuMainFileExit
            // 
            this.menuMainFileExit.Name = "menuMainFileExit";
            this.menuMainFileExit.Size = new System.Drawing.Size(135, 22);
            this.menuMainFileExit.Text = "E&xit";
            this.menuMainFileExit.Click += new System.EventHandler(this.menuMainFileExit_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainHelpAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // menuMainHelpAbout
            // 
            this.menuMainHelpAbout.Name = "menuMainHelpAbout";
            this.menuMainHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuMainHelpAbout.Text = "&About";
            this.menuMainHelpAbout.Click += new System.EventHandler(this.menuMainHelpAbout_Click);
            // 
            // ctxmenuProvKeywords
            // 
            this.ctxmenuProvKeywords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxmenuKeywordsCopyFlags});
            this.ctxmenuProvKeywords.Name = "ctxmenuProvKeywords";
            this.ctxmenuProvKeywords.Size = new System.Drawing.Size(173, 48);
            // 
            // ctxmenuKeywordsCopyFlags
            // 
            this.ctxmenuKeywordsCopyFlags.Name = "ctxmenuKeywordsCopyFlags";
            this.ctxmenuKeywordsCopyFlags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.ctxmenuKeywordsCopyFlags.Size = new System.Drawing.Size(172, 22);
            this.ctxmenuKeywordsCopyFlags.Text = "Copy Flags";
            this.ctxmenuKeywordsCopyFlags.Click += new System.EventHandler(this.ctxmenuKeywordsCopyFlags_Click);
            // 
            // WEPExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 742);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.gbProviderMetadata);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbProviderKeywords);
            this.Controls.Add(this.gbProviderFilters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WEPExplorer";
            this.Text = "Windows Events Providers Explorer";
            this.Load += new System.EventHandler(this.WEPExplorerForm_Load);
            this.gbProviderFilters.ResumeLayout(false);
            this.gbProviderFilters.PerformLayout();
            this.gbProviderKeywords.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbProviderMetadata.ResumeLayout(false);
            this.ctxmenuProvMetadata.ResumeLayout(false);
            this.ctxmenuProviders.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ctxmenuProvKeywords.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbProviderFilters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbProviderKeywords;
        private System.Windows.Forms.ListView lvProviderKeywords;
        private PresentationControls.CheckBoxComboBox cbchkOpcodes;
        private PresentationControls.CheckBoxComboBox cbchkLevels;
        private PresentationControls.CheckBoxComboBox cbchkChannels;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lvProviders;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox txtProviderNameGuidFilter;
        private System.Windows.Forms.Label lblProviderNameGuid;
        private System.Windows.Forms.GroupBox gbProviderMetadata;
        private System.Windows.Forms.ListView lvProviderMetadata;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataId;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataLevel;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataOpcode;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataTask;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataChannel;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataMessage;
        private System.Windows.Forms.ColumnHeader lvcKeywordsValue;
        private System.Windows.Forms.ColumnHeader lvcKeywordsName;
        private System.Windows.Forms.ColumnHeader lvcKeywordsMessage;
        private System.Windows.Forms.TextBox txtProviderFilterText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataKeyword;
        private System.Windows.Forms.Button btnProvFilterApply;
        private System.Windows.Forms.ContextMenuStrip ctxmenuProvMetadata;
        private System.Windows.Forms.ContextMenuStrip ctxmenuProviders;
        private PresentationControls.CheckBoxComboBox cbchkTasks;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuitemProvMetaInfo;
        private System.Windows.Forms.ColumnHeader lvcProvMetadataFields;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileExit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelpAbout;
        private System.Windows.Forms.ContextMenuStrip ctxmenuProvKeywords;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileClearCache;
        private PresentationControls.CheckBoxComboBox cbchkTemplateFields;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuitemProvMetaCopyID;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuitemProvMetaCopyIDAsCase;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuProviderCopyName;
        private System.Windows.Forms.ComboBox cbProviderMetadataTemplateFieldsMatchCondition;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuProviderCopyGuid;
        private System.Windows.Forms.ToolStripMenuItem ctxmenuKeywordsCopyFlags;
    }
}

