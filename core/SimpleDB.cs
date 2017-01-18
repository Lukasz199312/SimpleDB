using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using SimpleDB.core;
using System.Xml.Linq;
using SimpleDB.core.IO;
using SimpleDB.core.TableManagement;

namespace SimpleDB
{
    public class CoreDB
    {
        private XDocument xEntity;
        private XDocument xTableList;
        private Table workTable;

        private List<XDocument> xTables = new List<XDocument>();

        private static string path;
        private static CoreDB Database = null;
        //private string currentDb_Name;
        //private string currentDb_Path;
        private TableIO IO = new TableIO();

        public DatabaseCreator crator = new DatabaseCreator();
        public DatabaseLoader loader = new DatabaseLoader();
        public TableCreator tableCreator;
       // public TableListDetails tableListDetails;
        public TableList tableList;


        private CoreDB()
        {

        }

        public bool CreateDB(string path)
        {
            return crator.Create(path);
        }

        public bool LoadDB(string dbPath)
        {
            path = dbPath;
            loader.Load(dbPath, ref this.xEntity, ref this.xTableList);

            tableList = TableList.getTableList();
            tableCreator = new TableCreator(path);

            TableList.initializeXDocument(IO.LoadXMLTable("table_list", path), IO);
            ReferenceHelper.Initialize();

            return true;
        }

        public Table getTable(string tableName)
        {
            //return workTable = new Table(IO.LoadXMLTable(tableName, path), tableName);

            Table table = TableList.getTableList().getTable(tableName);
            return table;
        }

        public void RemoveTable(string tableName)
        {
            tableList.Release(tableName);
            tableList.removeTableFromList(tableName);
            tableList.SaveTable();
            IO.Delete(tableName, path);
            
        }


        public void CreateTable(string tableName)
        {
            this.tableCreator.Create(tableName);
        }

        public void Save()
        {
        }
            
        public static string getPath()
        {
            return path;
        }

        public static CoreDB getDatabase()
        {
            if(Database == null)
            {
                return Database = new CoreDB();
            }
            return Database;

        }
    }
}
