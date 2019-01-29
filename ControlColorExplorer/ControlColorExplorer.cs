using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmColorExplorer : Form
    {

        public Dictionary<Color, string> ColorNames = new Dictionary<Color, string>();

        public frmColorExplorer()
        {
            InitializeComponent();
            GetAllColors();
        }

        private void PopulateColors(bool bBackground)
        {
            int ColorRepeat;
            if (!int.TryParse(txtColorRepeat.Text, out ColorRepeat))
                ColorRepeat = 5;
            lvColors.BeginUpdate();
            lvColors.Items.Clear();
            AddColorsDynamically(bBackground, ColorRepeat);
            lvColors.EndUpdate();
        }

        private void GetAllColors()
        {
            ColorNames.Clear();
            var T = typeof(System.Drawing.Color);
            PropertyInfo[] properties = T.GetProperties();
            foreach (var property in properties)
            {
                if (!property.DeclaringType.Name.Equals("Color"))
                    continue;

                try
                {
                    var c = (Color)property.GetValue(T, null);
                    var n = property.Name;
                    ColorNames[c] = n;
                }
                catch
                {
                    break;
                }
            }
        }

        private void AddColoredItem(
            string Name,
            Color c,
            bool bBackground)
        {
            var lvi = new ListViewItem(Name);
            if (bBackground)
                lvi.BackColor = c;
            else
                lvi.ForeColor = c;

            lvColors.Items.Add(lvi);
        }

        private void AddColorsDynamically(
            bool bBackground,
            int ColorRepeat = 1)
        {
            var T = typeof(System.Drawing.Color);
            PropertyInfo[] properties = T.GetProperties();
            foreach (var property in properties)
            {
                if (!property.DeclaringType.Name.Equals("Color"))
                    continue;

                Color c;
                try
                {
                    c = (Color)property.GetValue(T, null);
                }
                catch
                {
                    break;
                }
                var n = property.Name;

                for (int i=0;i<ColorRepeat;i++)
                    AddColoredItem(n, c, bBackground);
            }
        }

        private void btnRenderFore_Click(
            object sender, 
            EventArgs e)
        {
            PopulateColors(false);
        }

        private void frmColorExplorer_Load(
            object sender, 
            EventArgs e)
        {
            PopulateColors(false);
        }

        private void btnRenderBack_Click(
            object sender, 
            EventArgs e)
        {
            PopulateColors(true);
        }

        private void btnCopyColorNames_Click(object sender, EventArgs e)
        {
            var Colors = new List<string>();
            foreach (ListViewItem lvi in lvColors.SelectedItems)
                Colors.Add(lvi.Text);

            if (Colors.Count > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText(string.Join(", ", Colors));
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            @"ControlColorExplorer

Explorer the colors of a windows control, design your application better!

(c) Elias Bachaalany <lallousz-x86@yahoo.com>
", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
