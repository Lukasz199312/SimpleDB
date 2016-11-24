using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleDB.core
{
    public class Line
    {
        public List<Row> Rows { get; }
        private XElement xLine;

        public Line(Column[] columns, XElement xLine)
        {
            Rows = new List<Row>();

            this.xLine = xLine;
            foreach(Column col in columns)
            {
                Rows.Add(new Row(this.xLine, col));
            }
        }

        public Line(Column[] columns, XElement[] elements, XElement xLine)
        {
            Rows = new List<Row>();

            this.xLine = xLine;

            int index = 0;
            foreach(Column col in columns)
            {
                Rows.Add(new Row(elements[index], col, elements[index].Value));
                index++;
            }
        }

        public Row getRow(object columnName)
        {
            return Rows.First(row => row.parentColumn.columnName == columnName.ToString());
        }

    }
}
