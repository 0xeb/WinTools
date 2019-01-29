using System;
using System.Collections;
using System.Windows.Forms;

namespace WEPExplorer
{
    public class SimpleListViewItemComparer : IComparer
    {
        private int m_iCol;
        private bool m_bAsc = true;
        private bool m_bIsNumeric = false;

        public bool Ascending
        {
            get { return m_bAsc; }
            set { m_bAsc = value; }
        }

        public int Column
        {
            get { return m_iCol; }
            set { m_iCol = value; }
        }

        public bool Numeric
        {
            get { return m_bIsNumeric; }
            set { m_bIsNumeric = value; }
        }

        public SimpleListViewItemComparer(int columnIndex)
        {
            Column = columnIndex;
        }

        public int Compare(object x, object y)
        {
            ListViewItem itemX = x as ListViewItem;
            ListViewItem itemY = y as ListViewItem;

            if (itemX == null && itemY == null)
                return 0;
            else if (itemX == null)
                return -1;
            else if (itemY == null)
                return 1;

            if (itemX == itemY)
                return 0;

            int Result;

            if (Numeric)
            {
                decimal itemXVal, itemYVal;

                if (!Decimal.TryParse(itemX.SubItems[Column].Text, out itemXVal))
                    itemXVal = 0;

                if (!Decimal.TryParse(itemY.SubItems[Column].Text, out itemYVal))
                    itemYVal = 0;

                Result = Decimal.Compare(itemXVal, itemYVal);
            }
            else
            {
                string itemXText = itemX.SubItems[Column].Text;
                string itemYText = itemY.SubItems[Column].Text;
                Result = String.Compare(itemXText, itemYText);
            }
            if (Ascending)
                return Result;
            else
                return -Result;
        }
    }
}
