namespace WindowsFormsApplication1
{
    partial class frmColorExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmColorExplorer));
            this.btnRenderFore = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvColors = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRenderBack = new System.Windows.Forms.Button();
            this.txtColorRepeat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopyColorNames = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRenderFore
            // 
            this.btnRenderFore.Location = new System.Drawing.Point(12, 12);
            this.btnRenderFore.Name = "btnRenderFore";
            this.btnRenderFore.Size = new System.Drawing.Size(75, 55);
            this.btnRenderFore.TabIndex = 9;
            this.btnRenderFore.Text = "Fore Color";
            this.btnRenderFore.UseVisualStyleBackColor = true;
            this.btnRenderFore.Click += new System.EventHandler(this.btnRenderFore_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvColors);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 402);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color rendering";
            // 
            // lvColors
            // 
            this.lvColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvColors.FullRowSelect = true;
            this.lvColors.Location = new System.Drawing.Point(3, 16);
            this.lvColors.Name = "lvColors";
            this.lvColors.Size = new System.Drawing.Size(354, 383);
            this.lvColors.TabIndex = 10;
            this.lvColors.UseCompatibleStateImageBehavior = false;
            this.lvColors.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "COLOR";
            this.columnHeader1.Width = 300;
            // 
            // btnRenderBack
            // 
            this.btnRenderBack.Location = new System.Drawing.Point(12, 73);
            this.btnRenderBack.Name = "btnRenderBack";
            this.btnRenderBack.Size = new System.Drawing.Size(75, 55);
            this.btnRenderBack.TabIndex = 11;
            this.btnRenderBack.Text = "Back Color";
            this.btnRenderBack.UseVisualStyleBackColor = true;
            this.btnRenderBack.Click += new System.EventHandler(this.btnRenderBack_Click);
            // 
            // txtColorRepeat
            // 
            this.txtColorRepeat.Location = new System.Drawing.Point(106, 28);
            this.txtColorRepeat.Name = "txtColorRepeat";
            this.txtColorRepeat.Size = new System.Drawing.Size(64, 20);
            this.txtColorRepeat.TabIndex = 12;
            this.txtColorRepeat.Text = "4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Color repeat:";
            // 
            // btnCopyColorNames
            // 
            this.btnCopyColorNames.Location = new System.Drawing.Point(106, 73);
            this.btnCopyColorNames.Name = "btnCopyColorNames";
            this.btnCopyColorNames.Size = new System.Drawing.Size(75, 55);
            this.btnCopyColorNames.TabIndex = 14;
            this.btnCopyColorNames.Text = "Copy Color names";
            this.btnCopyColorNames.UseVisualStyleBackColor = true;
            this.btnCopyColorNames.Click += new System.EventHandler(this.btnCopyColorNames_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(202, 73);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 55);
            this.btnAbout.TabIndex = 15;
            this.btnAbout.Text = "&About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // frmColorExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 546);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnCopyColorNames);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtColorRepeat);
            this.Controls.Add(this.btnRenderBack);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRenderFore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmColorExplorer";
            this.Text = "Control Color Explorer";
            this.Load += new System.EventHandler(this.frmColorExplorer_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRenderFore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView lvColors;
        private System.Windows.Forms.Button btnRenderBack;
        private System.Windows.Forms.TextBox txtColorRepeat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopyColorNames;
        private System.Windows.Forms.Button btnAbout;
    }
}

