using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyboardSpeedReset
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        public Form1()
        {
            InitializeComponent();

            //Handle the PowerModeChangedEvent
            Microsoft.Win32.SystemEvents.PowerModeChanged += new Microsoft.Win32.PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        void SystemEvents_PowerModeChanged(
            object sender, 
            Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            if ( e.Mode == Microsoft.Win32.PowerModes.Resume)
                DoIt();
        }

        private void DoIt()
        {
            // resume from standby event
            uint SPI_SETKEYBOARDSPEED = 0x000B;
            uint SPI_SETKEYBOARDDELAY = 0x0017;
            uint ReptSpeed = 31;
            uint Delay = 0;
            uint NotUsed = 0;

            SystemParametersInfo(
                SPI_SETKEYBOARDSPEED, 
                ReptSpeed, 
                ref NotUsed, 
                0);
            SystemParametersInfo(
                SPI_SETKEYBOARDDELAY, 
                Delay, 
                ref NotUsed, 
                0);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void TrayIcone_MouseDoubleClick(
            object sender, 
            MouseEventArgs e)
        {
            DoIt();
        }
    }
}
