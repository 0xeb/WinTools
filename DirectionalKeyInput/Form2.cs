using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DirInput
{
  public partial class Form2 : Form
  {
    public Form2()
    {
      InitializeComponent();
    }

    public Control GetEditControl()
    {
      return textBox1;
    }

    private void Form2_Load(object sender, EventArgs e)
    {

    }

  }
}