using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace SimpleDB.core.TableManagement
{
    public class TableDetails
    {
        private const int MAX_TABLE_NAME = 32;

        public XElement AddEntity(XDocument xTable, string columnName, DataType type, DataOptions[] optionsData = null)
        {
            columnName = CheckTableNameLength(columnName);
            if (xTable.Root.Element("Entity").Elements().Where(element => element.Attribute("Name").Value == columnName).Count() >= 1) return null;
            

            var data = new XElement("Column",
                       new XAttribute("Name", columnName),
                       new XAttribute("DataType", type));

            if(optionsData != null)
            {
                foreach(DataOptions opt in optionsData)
                {
                    data.Add(new XAttribute(opt.ToString(), ""));
                }
            }

            xTable.Root.Element("Entity").Add(data);
            return data;
        }

        private string CheckTableNameLength(string tableName)
        {
            if(tableName.Length > MAX_TABLE_NAME)
            {
                return tableName.Substring(0, MAX_TABLE_NAME - 1);
            }
            return tableName;
        }

        public XElement getXmlColumn(string columnName, XDocument xTable)
        {
            return xTable.Root.Element("Entity").Elements().Where(el => el.Attribute("Name").Value == columnName).First();
        }

        public IEnumerable<XElement> getXmlColumns(XDocument xTable)
        {
            if (xTable.Root.Element("Entity").Elements().Count() <= 0) return null;
            return xTable.Root.Element("Entity").Elements();
        }

        public IEnumerable<XElement> getXmlLines(XDocument xTable)
        {
            if (xTable.Root.Element("Content").Elements().Count() <= 0) return null;
            return xTable.Root.Element("Content").Elements();
        }

        public string[] getColumnesName(XDocument xTable)
        {
            IEnumerable<XElement> columnesElement = xTable.Root.Element("Entity").Elements();
            string[] ColumnNames = new string[columnesElement.Count()];

            int Index = 0;
            foreach(XElement el in columnesElement)
            {
                ColumnNames[Index] = el.Attribute("Name").Value;
                Index++;
            }
            return ColumnNames;
        }

        public XElement addLine(XDocument xTable)
        {
            int val = Convert.ToInt32(xTable.Root.Element("Head").Value);
            val ++;

            XElement element = new XElement("Field", new XAttribute("_id", val));

            xTable.Root.Element("Head").Value = val.ToString();
            xTable.Root.Element("Content").Add(element);

            return element;

        }

    }
}
