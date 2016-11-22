using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace SimpleDB.core.IO
{
    public class DatabaseLoader
    {
        public bool Load(string dbPath, ref XDocument xEntity, ref XDocument xTableList)
        {
            if (dbPath[dbPath.Length - 1] != '/') dbPath += '/';

            xEntity = XDocument.Load(dbPath + "entity.xml");
            xTableList = XDocument.Load(dbPath + "table_list.xml");

            return true;
        }
    }
}
