using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleDB.core
{
    public class Reference
    {
        public Column parentColumn;
       // public Reference ForeigenReference;

        private XAttribute xReferenceColumn;
        private XAttribute xReferenceTable;
        private XElement xReference;

        public Reference(XElement element, Column column)
        {
            this.xReference = element;
            xReferenceColumn = xReference.Attribute("Column");
            xReferenceTable = xReference.Attribute("Table");
            parentColumn = column;
        }

        public void setColumnName(string name)
        {
            xReferenceColumn.Value = name;
        }

        public void setTableName(string name)
        {
            xReferenceTable.Value = name;
        }

        public string getColumnName()
        {
            return xReferenceColumn.Value;
        }

        public string getTableName()
        {
            return xReferenceTable.Value;
        }

        public void Remove()
        {
            xReference.Remove();
        }

    }
}
