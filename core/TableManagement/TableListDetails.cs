using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SimpleDB.core.IO;

namespace SimpleDB.core.TableManagement
{
    public class TableListDetails
    {
        private XDocument xTableList;

        public TableListDetails(XDocument xTableList)
        {
            if (xTableList == null) throw new ArgumentNullException();
            this.xTableList = xTableList;
        }

        public void addTable(string tableName)
        {
            int tableID = getCurrentId();
            tableID++;
            increaseId(tableID.ToString());

            xTableList.Root.Add(getTableSchema(tableID.ToString(), tableName));
        }

        public int getCurrentId()
        {
            return Convert.ToInt32(xTableList.Root.Element("Head").Value);
        }

        public XDocument getTableList()
        {
            return this.xTableList;
        }

        private object getTableSchema(string idValue, string tableName)
        {
            return new XElement("Table",
                                        new XAttribute("id", idValue),
                                        new XAttribute("name", tableName));
        }

        private void increaseId(string newIdValue)
        {
            xTableList.Root.Element("Head").Value = newIdValue;
        }

        public List<string> getTablesName()
        {
            List<string> tableList = null;

            IEnumerable<XAttribute> xDocAttribute = xTableList.Root.Elements("Table").Attributes("name");
            
            foreach(XAttribute xAttr in xDocAttribute)
            {
                tableList.Add(xAttr.Value);
            }

            return tableList;
        }

    }
}
