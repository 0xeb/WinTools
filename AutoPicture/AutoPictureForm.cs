using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutoPictureApp
{
    public partial class AutoPictureForm : Form
    {
        string _filename;

        public AutoPictureForm(string filename)
        {
            _filename = filename;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path = Path.GetDirectoryName(_filename);
            fileSystemWatcher1.Filter = Path.GetFileName(_filename);
            BackgroundImageLayout = ImageLayout.Stretch;
            refresh_image();
        }

        private void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            refresh_image();
        }

        private void refresh_image()
        {
            if (File.Exists(_filename))
            {
                var img = Image.FromStream(new MemoryStream(File.ReadAllBytes(_filename)));
                BackgroundImage = img;
                int dx = Width - ClientRectangle.Width;
                int dy = Height - ClientRectangle.Height;
                Width = img.Width + dx;
                Height = img.Height + dy;
                Text = string.Format("'{0}' loaded @ {1}", _filename, File.GetLastWriteTime(_filename));
            }
            else
            {
                Text = String.Format("'{0} does not exist!", _filename);
            }
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            refresh_image();
        }
    }
}
