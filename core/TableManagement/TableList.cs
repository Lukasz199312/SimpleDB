using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SimpleDB.core.IO;

namespace SimpleDB.core.TableManagement
{
    public class TableList
    {
        private static TableList tableList = null;
        private static XDocument xTable;

        private List<Table> Tables = new List<Table>();
        private XAttribute headElement;
        private TableIO IO = new TableIO();

        public List<string> Names { get; private set; } = new List<string>();
        

        private TableList() { }

        private void Initialize()
        {
            var tables = xTable.Root.Elements();
            
            foreach(var table in tables)
            {
              Names.Add(table.Value);
            }

        }

        public Table loadTable(string tableName)
        {
            Table table = new Table(IO.LoadXMLTable(tableName, CoreDB.getPath()), tableName);

            Tables.Add(table);
            return table;
        }

        public Table getTable(string tableName)
        {
            try
            {
                return Tables.First(table => table.tableName == tableName);
            }
            catch
            {
                return loadTable(tableName);
            }
        }

        public bool Release(string tableName)
        {
            Table tableToRemove = Tables.First(table => table.tableName == tableName);
            var result = Tables.Remove(tableToRemove);
            tableToRemove = null;
            Names.Remove(tableName);
            return result;
        }

        public IEnumerable<string> tablesInMemory()
        {
            List<string> tablesMem = new List<string>();
            foreach(Table table in Tables)
            {
                tablesMem.Add(table.tableName);
            }
            return tablesMem;
        }

        public static TableList getTableList()
        {
            if (tableList == null)
            {
                tableList = new TableList();
            }
            return tableList;
        }

        internal void Rename(string oldName, string newName)
        {
            xTable.Root.Elements().First(el => el.Value == oldName).Value = newName;
            SaveTable();
        }

        internal void addReferencePrefix(string tableName)
        {
            xTable.Root.Elements().First(element => element.Value == tableName).Add(new XAttribute("FOREIGN_KEY",""));
            new TableIO().Save("table_list", CoreDB.getPath(), xTable);
        }

        internal void removeReferencePrefix(string tableName)
        {
            try
            {
                xTable.Root.Elements().First(element => element.Value == tableName).Attribute("FOREIGN_KEY").Remove();
                new TableIO().Save("table_list", CoreDB.getPath(), xTable);
            }
            catch { }
        }

        internal void addTableNameToList(string tableName)
        {
            IncreaseHead();
            xTable.Root.Add(new XElement("Table", tableName));
            Names.Add(tableName);
            new TableIO().Save("table_list", CoreDB.getPath(), xTable);
        }

        internal void removeTableFromList(string tableName)
        {
            DecreaseHead();
            xTable.Root.Elements().First(el => el.Value == tableName).Remove();
        }

        internal IEnumerable<string> getTablesNamesWithReferences()
        {
            var elements = xTable.Root.Elements().Where(element => element.Attribute("FOREIGN_KEY") != null);
            List<string> tmpList = new List<string>();

            foreach(var ele in elements)
            {
                tmpList.Add(ele.Value);
            }

            return tmpList;
        }

        internal int getHeadID()
        {
            return Convert.ToInt32(headElement.Value);
        }

        internal void IncreaseHead()
        {
            headElement.Value = (Convert.ToInt32(headElement.Value) + 1).ToString();
        }

        internal void DecreaseHead()
        {
            headElement.Value = (Convert.ToInt32(headElement.Value) - 1).ToString();
        }

        internal static void initializeXDocument(XDocument document, TableIO IO)
        {
            xTable = document;
            getTableList().Initialize();
            getTableList().headElement = xTable.Root.Attribute("id");

        }

        internal void SaveTable()
        {
            new TableIO().Save("table_list", CoreDB.getPath(), xTable);
        }


    }
}
