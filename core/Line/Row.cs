using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleDB.core
{
    public class Row
    {
        public Column parentColumn { get; }

        private XElement xRow = new XElement("Row");
        private XCData xData = new XCData("");

        public Row(XElement xLine, Column parentColumn)
        {
            xRow.Add(xData);
            xLine.Add(xRow);

            this.parentColumn = parentColumn;
        }

        public Row(XElement xRow, Column parentColumn, string Value)
        {
            this.xRow = xRow;
            xData = (XCData)xRow.FirstNode;
            this.parentColumn = parentColumn;
        }


        public string get()
        {
            return xRow.Value;
        }

        public void set(object data)
        {
            xData.Value = data.ToString();
        }

        public void clear()
        {
            xData.Value = "";
        }
    }
}
