using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DirInput
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string [] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      KeyboardForm f = new KeyboardForm();
      f.args = args;
      Application.Run(f);
    }
  }
}