using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

/* ----------------------------------------------------------------------------- 
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
* ----------------------------------------------------------------------------- 
*/
namespace Explore
{
    public static class XmlExtensions
    {
        static public string GetAttrValue(
            this XmlNode node, 
            string AttrName)
        {
            try
            {
                return node.Attributes[AttrName].Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool SerializeToXmlFile(
            this object obj,
            string FileName,
            out string ExMessage)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(FileName))
                {
                    XmlSerializer ser = new XmlSerializer(obj.GetType());
                    ser.Serialize(writer, obj);
                }
                ExMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ExMessage = ex.ToString();
                Debug.WriteLine(
                    string.Format("SerializeToXmlFile failed to serialize {0} of the type {1}.\n" +
                                  "Exception: {2}",
                                FileName,
                                obj.GetType().Name,
                                ExMessage));
                return false;
            }
        }

        public static bool SerializeToXmlFile(
            this object obj,
            string FileName)
        {
            string ExMessage;
            return obj.SerializeToXmlFile(FileName, out ExMessage);
        }

        public static T DeserializeFromXmlFile<T>(
            string FileName,
            out string ExMessage) where T : class
        {
            try
            {
                using (TextReader reader = new StreamReader(FileName))
                {
                    ExMessage = null;
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    return ser.Deserialize(reader) as T;
                }
            }
            catch (Exception ex)
            {
                ExMessage = ex.ToString();
                Debug.WriteLine(
                    string.Format("DeserializeFromFile failed to deserialize {0} of the type {1}.\n" +
                                  "Exception: {2}",
                                FileName,
                                typeof(T).Name,
                                ExMessage));
                return default(T);
            }
        }

        public static T DeserializeFromXmlFile<T>(string FileName) where T : class
        {
            string ExMessage;
            return DeserializeFromXmlFile<T>(FileName, out ExMessage);
        }

    }

    public static class Extensions
    {
        static public bool HasAttribute(this Type t, Type Attr)
        {
            return t.GetCustomAttributes(Attr, true).Any();
        }
    }

    public static class ListViewExtensions
    {
        public static string GetItemsString(
            this System.Windows.Forms.ListViewItem lvi,
            string SurroundL = "\"",
            string SurroundR = "\"",
            string Join = "\t")
        {
            List<string> s = new List<string>();
            foreach (System.Windows.Forms.ListViewItem.ListViewSubItem CurSub in lvi.SubItems)
                s.Add(string.Format("{0}{1}{2}", SurroundL, CurSub.Text, SurroundR));

            return string.Join(Join, s);
        }

        public static void SelectAll(
            this System.Windows.Forms.ListView lv)
        {
            lv.BeginUpdate();
            foreach (System.Windows.Forms.ListViewItem lvi in lv.Items)
                lvi.Selected = true;
            lv.EndUpdate();
        }
    }
}
