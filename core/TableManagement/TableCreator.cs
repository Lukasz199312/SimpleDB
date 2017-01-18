using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDB.core.IO;
using System.Xml.Linq;

namespace SimpleDB.core.TableManagement
{
    public class TableCreator
    {
        private XDocument xTableList;
        private string path;

        public TableCreator(string path)
        {

            this.path = path;
        }

        public XDocument Create(string tableName)
        {
            TableIO tableIO = new TableIO();
            XDocument XNewDoc = null;

            if (tableIO.isExists(tableName, path)) // Load exsist table
            {
                XNewDoc = tableIO.LoadXMLTable(tableName, path);

            }

            else //Create new table and load it
            {
                if (tableIO.CreateTable(tableName, path)) XNewDoc = tableIO.LoadXMLTable(tableName, path);

                addTableName(XNewDoc, tableName);
                addTableID(XNewDoc, TableList.getTableList().getHeadID());

                TableList.getTableList().addTableNameToList(tableName);
                tableIO.Save(tableName, path, XNewDoc);
            }

            return XNewDoc;
        }

        private void addTableName(XDocument xdoc, string tableName)
        {
            xdoc.Root.Attribute("Name").Value = tableName;
        }

        private void addTableID(XDocument xdoc, int id)
        {
            xdoc.Root.Attribute("Id").Value = id.ToString();
        }
        
    }
}
