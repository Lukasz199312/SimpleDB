using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SimpleDB.core.TableManagement;

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

        public Line getReferenceLine()
        {
            foreach(DataOptions opt in parentColumn.dataOptions)
            {
                if (opt == DataOptions.FOREIGN_KEY)
                {
                   return TableList.getTableList().getTable(parentColumn.getReference().getTableName()).getLine(get(), parentColumn.columnName);
                }
            }

            return null;
        }

        public void set(object data)
        {
            xData.Value = data.ToString();
        }

        public void clear()
        {
            xData.Value = "";
        }

        internal XElement getRow()
        {
            return xRow;
        }

        internal void remove()
        {
            xRow.Remove();
        }

    }
}
