using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace SimpleDB.core.IO
{
    public class TableIO
    {
        public bool CreateTable(string tableName, string path)
        {
            path = MakePath(tableName, path);

            if (File.Exists(path)) return false;

            File.Copy("xmls/table.xml", path);
            return true;
        }

        public XDocument LoadXMLTable(string tableName, string path)
        {
            path = MakePath(tableName, path);

            return XDocument.Load(path);
        }

        public bool isExists(string tableName, string path)
        {
            path = MakePath(tableName, path);

            if (File.Exists(path)) return true;
            else return false;
        }

        public string MakePath(string tableName, string path)
        {
            if (path[path.Length - 1] != '/') path += '/';
            return path + tableName + ".xml";
        }


        public void Save(string xDocumentName, string path, XDocument xDocument)
        {
            path = MakePath(xDocumentName, path);
            xDocument.Save(path);
        }
    }
}
