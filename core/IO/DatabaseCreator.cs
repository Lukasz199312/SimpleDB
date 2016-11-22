using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpleDB.core.IO
{
    public class DatabaseCreator
    {
        public bool Create(string Path)
        {
            if (Path[Path.Length - 1] != '/') Path += '/';

            if (File.Exists(Path + "entity.xml")) return false;
            if (File.Exists(Path + "table.xml")) return false;
            if (File.Exists(Path + "table_list.xml")) return false;
             
            File.Copy(@"xmls/entity.xml", Path + "entity.xml");
            File.Copy(@"xmls/table.xml", Path + "table.xml");
            File.Copy(@"xmls/table_list.xml", Path + "table_list.xml");

            return true;
        }
    }
}
