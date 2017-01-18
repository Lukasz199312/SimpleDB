using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDB.core.TableManagement;

namespace SimpleDB.core
{
    internal static class ReferenceHelper
    {
        public static void Initialize()
        {
            TableList list = TableList.getTableList();

            foreach(string tableName in list.Names)
            {
                Table table = list.getTable(tableName);
                var result = list.getTablesNamesWithReferences().Where(name => name == table.tableName);
                if(result != null)
                {
                    foreach(Column column in table.getColumns())
                    {
                        foreach(DataOptions data in column.dataOptions)
                        {
                            if(data == DataOptions.FOREIGN_KEY)
                            {
                                column.InitializeReference();
                            }
                        }
                    }
                }
            }
        }

        public static void editName(Reference reference, string oldName, string newName)
        {
            TableList tablelist = TableList.getTableList();
            Table table = tablelist.getTable(reference.getTableName());

            table.getColumn(oldName).getReference().setColumnName(newName);

        }
    }
}
