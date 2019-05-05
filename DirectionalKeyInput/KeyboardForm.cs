using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;

/*
Directional Key input by Elias Bachaalany <lallousz-x86@yahoo.com>


 * Copyright (c) Elias Bachaalany <lallousz-x86@yahoo.com>
 * All rights reserved.
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
*/

namespace DirInput
{
    public partial class KeyboardForm : Form
    {
        public string[] args;
        enum Directions : int
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        int nDir = 0, nLastDir = 0;

        Hashtable DirKeyNames = new Hashtable();
        Hashtable KeyGroups = new Hashtable();
        string InputKeys;
        Control EditControl = null;

        public KeyboardForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fn = "";
            if (args.Length == 0)
                fn = "mapping.txt";
            else
                fn = args[0];

            if (!parseKeys(fn))
            {
                MessageBox.Show("Cannot find mappings file " + fn);
                this.Close();
                return;
            }
            NotepadForm f = new NotepadForm();
            EditControl = f.GetEditControl();
            f.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                nDir |= (int)Directions.Up;
            }
            else if (e.KeyCode == Keys.Down)
            {
                nDir |= (int)Directions.Down;
            }
            else if (e.KeyCode == Keys.Left)
            {
                nDir |= (int)Directions.Left;
            }
            else if (e.KeyCode == Keys.Right)
            {
                nDir |= (int)Directions.Right;
            }
            else
            {
                //Text = string.Format("{0} n:{1}", e.KeyCode.ToString(), DirKeyNames[nDir]);
                return;
            }
            if (nLastDir != nDir)
            {
                DoDirChanged(nLastDir);
            }

            nLastDir = nDir;
            //Text = string.Format("{0:x} n:{1}", nDir, DirKeyNames[nDir]);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                nDir &= ~(int)Directions.Up;
            }
            else if (e.KeyCode == Keys.Down)
            {
                nDir &= ~(int)Directions.Down;
            }
            else if (e.KeyCode == Keys.Left)
            {
                nDir &= ~(int)Directions.Left;
            }
            else if (e.KeyCode == Keys.Right)
            {
                nDir &= ~(int)Directions.Right;
            }
            else
            {
                DoHandleKey(e.KeyCode);
                //Text = string.Format("{0} n:{1}", e.KeyCode.ToString(), DirKeyNames[nDir]);
                return;
            }
            //Text = string.Format("{0:x} n:{1}", nDir, DirKeyNames[nDir]);
            DoDirChanged(0);
            nLastDir = 0;
        }

        private void setStatus(string s)
        {
            lblStatus.Text = s;
        }

        private void DoHandleKey(Keys key)
        {
            if (key == Keys.Escape)
            {
                EditControl.Text = "";
                return;
            }
            else if (key == Keys.Back)
            {
                int L = EditControl.Text.Length;
                if (L == 0)
                    return;

                EditControl.Text = EditControl.Text.Substring(0, L - 1);
                return;
            }
            else if (key == Keys.Space)
            {
                EditControl.Text += " ";
                return;
            }
            if (nDir == 0)
                return;

            int pos = InputKeys.IndexOf(key.ToString());
            if (pos == -1)
                return;

            string s = KeyGroups[nDir].ToString();
            if (s.Length == 0 || pos >= s.Length)
                return;

            setStatus(string.Format("pos={0}", pos));
            EditControl.Text += s[pos];
        }

        private void DoDirChanged(int LastDir)
        {
            if (nDir == 0)
            {
                lblGroupValues.Text = lblGroupName.Text = lblStatus.Text = "";
                return;
            }
            lblGroupName.Text = "Group:" + DirKeyNames[nDir].ToString();
            lblGroupValues.Text = "Group Values:" + KeyGroups[nDir].ToString();
        }

        bool parseKeys(string filename)
        {
            DirKeyNames[1] = "U";
            DirKeyNames[2] = "D";

            DirKeyNames[3] = "UD";

            DirKeyNames[4] = "L";
            DirKeyNames[5] = "UL";

            DirKeyNames[6] = "DL";
            DirKeyNames[7] = "UDL";
            DirKeyNames[8] = "R";
            DirKeyNames[9] = "UR";
            DirKeyNames[10] = "DR";
            DirKeyNames[11] = "UDR";
            DirKeyNames[12] = "LR";
            DirKeyNames[13] = "ULR";
            DirKeyNames[14] = "DLR";
            DirKeyNames[15] = "UDLR";

            StreamReader f = null;

            try
            {
                f = new StreamReader(filename);
            }
            catch
            {
                return false;
            }

            Hashtable KeyNameToInt = new Hashtable();
            foreach (DictionaryEntry de in DirKeyNames)
            {
                KeyNameToInt[de.Value] = de.Key;
                KeyGroups[de.Key] = "";
            }

            string line;
            char[] seps = new char[] { ':' };
            while ((line = f.ReadLine()) != null)
            {
                // Skip empty or comment lines
                line = line.Trim();
                if (line.Length == 0 || line[0] == ';')
                    continue;

                string[] parts = line.Split(seps);
                if (parts.Length != 2)
                    continue;

                // Parse the input keys
                if (parts[0].Equals("KEYS"))
                {
                    InputKeys = parts[1];
                    lblInputKeys.Text = string.Format("Input Keys: {0}", InputKeys);
                    continue;
                }

                try
                {
                    KeyGroups[KeyNameToInt[parts[0]]] = parts[1];
                    Controls["lbl" + parts[0]].Text = parts[1];
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    // Skip unknown
                    continue;
                }
            }
            f.Close();
            return true;
        }
    }
}