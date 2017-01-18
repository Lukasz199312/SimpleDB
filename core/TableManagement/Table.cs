using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SimpleDB.core.IO;
using SimpleDB.core;

namespace SimpleDB.core.TableManagement
{
    public class Table
    {
        public string tableName;
        public List<string> ReferenceColumn = new List<string>();

        private Entity entity;
        private string path;
        private List<Column> Columns;
        private List<Line> Lines = new List<Line>();

        private XDocument _xDocument;
        public XDocument xDocument { get { return this._xDocument;  } }

        public Table(XDocument xdoc, string tableName)
        {
            Columns = new List<Column>();
            this._xDocument = xdoc;
            path = CoreDB.getPath();
            this.tableName = tableName;
            entity = new Entity(_xDocument.Root.Element("Entity"));
            InitializeColumns();
            InitializeLines();
            InitializeColumnLine();
        }

        private void addEntity(string columnName, DataType dataType, DataOptions[] dataOptions = null)
        {
            new TableDetails().AddEntity(this.xDocument, columnName, dataType, dataOptions);
            //Columns.Add(new Column(columnName, dataType, dataOptions));
        }

        public bool addColumn(string columnName, DataType dataType, params DataOptions[] dataOptions)
        {
            XElement element =  new TableDetails().AddEntity(this.xDocument, columnName, dataType, dataOptions);
            Column col;

            if (element == null)
            {
                Column column = Columns.First(c => c.columnName == columnName);
                column.EditDataType(dataType);

                if( dataOptions.Contains(DataOptions.FOREIGN_KEY) == false )
                {
                    if(column.dataOptions.Contains(DataOptions.FOREIGN_KEY))
                    {
                        column.RemoveReference();
                        
                    }
                }

                col = column;



            }
            else col = new Column(columnName, dataType, dataOptions, element, this);


            Columns.Add(col);

            if(Columns.Count() - 1 >= 0)
            {
                foreach(Line lin in Lines)
                {
                    lin.addEmpty(col);
                }
            }

            return true;
        }

        private void EditExsistColumn(string columnName, DataType dataType, params DataOptions[] dataOptions)
        {
            Column selected = Columns.First(col => col.columnName == columnName);
            selected.EditDataType(dataType);
        }


        public Column getColumn(string tableName)
        {
            try
            {
                return Columns.First(Col => Col.columnName == tableName);
            } catch { return null;  }
           
        }

        public IEnumerable<Column> getColumns()
        {
            return Columns;
        }

        public Table getReferenceTable(string columnName)
        {
            return null;
        }

        public bool RemoveColumn(string columnName)
        {
            try
            {
                Column column = Columns.First(col => col.columnName == columnName);
                if (column == null) return false;

                column.Remove();
                column.columnLine.RemoveAllRows();
                Columns.Remove(column);
                return true;
            }
            catch { return false; }

        }

        public string[] getColumnNames()
        {
            return new TableDetails().getColumnesName(xDocument);
        }

        public List<string> getColumnNamesList()
        {
            return new TableDetails().getColumnesName(xDocument).ToList<string>();
        }

        public Entity getEntities()
        {
            return entity;
        }

        public void Save()
        {
            new TableIO().Save(tableName, path, xDocument);
        }

        public void Rename(string newTableName)
        {
            System.IO.File.Move(new TableIO().MakePath(tableName, path), new TableIO().MakePath(newTableName, path));
            TableList.getTableList().Rename(tableName, newTableName);
            tableName = newTableName;

            xDocument.Root.Attribute("Name").Value = newTableName;
            
        }

        public Line addLine()
        {
            XElement xLine = new TableDetails().addLine(xDocument);
            var line = new Line(Columns.ToArray(), xLine);
            Lines.Add(line);

            return line;

        }

        public Line addLine(params object[] data)
        {
            if (data.Count() != Columns.Count()) return null;

            XElement xLine = new TableDetails().addLine(xDocument);
            Line newLine = new Line(Columns.ToArray(), xLine);
            Lines.Add(newLine);

            int i = 0;
            foreach(object value in data)
            {
                newLine.Rows[i].set(value);
                i++;
            }

            return newLine;
        }

        public Row getRow(object RowValue, string columnName)
        {
            try
            {
                Line lin = getLine(RowValue, columnName);
                return lin.Rows.First(row => row.parentColumn.columnName == columnName);
            } catch
            {
                return null;
            }

        }

        public Line getReferenceLine(object RowValue, string columnName)
        {
            return getLine(RowValue, columnName).getRow(columnName).getReferenceLine();
        }

        public Line getLine(object RowValue, string columnName)
        {
            try
            {
                return Lines.First(line => line.getRow(columnName.ToString()).get() == RowValue.ToString());
            }
            catch { return null; }
        }

        public IEnumerable<Line> getLines(object RowValue, string columnName)
        {
            try
            {
                return Lines.Where(line => line.getRow(columnName.ToString()).get() == RowValue.ToString());
            }
            catch { return null; }
        }

        public Line[] getAllLines()
        {
            return Lines.ToArray();
        }

        private void InitializeColumns()
        {
            TableDetails tableDetails = new TableDetails();

            IEnumerable<XElement> xCol = tableDetails.getXmlColumns(xDocument);
            if (xCol == null)
            {
                return;
            }

            foreach(XElement el in xCol)
            {
                Columns.Add(xDocToColumn.tryParse(el,this));
            }
        }

        private void InitializeLines()
        {
            TableDetails tableDetails = new TableDetails();

            IEnumerable<XElement> XLine = tableDetails.getXmlLines(xDocument);
            if (XLine == null) return;

            foreach(XElement line in XLine)
            {
                Lines.Add( new Line(Columns.ToArray(), line.Elements().ToArray(), line) );
            }
        }

        private void InitializeColumnLine()
        {
            int i = 0;
            foreach(Column col in Columns)
            {
                foreach(Line line in Lines)
                {
                    col.columnLine.Rows.Add( line.getRows()[i] );
                }
                i++;
            }

        }


        public bool hasReference()
        {
            if (ReferenceColumn.Count <= 0) return false;
            else return true;
        }

    }
}
