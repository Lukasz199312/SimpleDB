﻿using System;
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
        }

        private void addEntity(string columnName, DataType dataType, DataOptions[] dataOptions = null)
        {
            new TableDetails().AddEntity(this.xDocument, columnName, dataType, dataOptions);
            //Columns.Add(new Column(columnName, dataType, dataOptions));
        }

        public bool addColumn(string columnName, DataType dataType, DataOptions[] dataOptions = null)
        {
            XElement element =  new TableDetails().AddEntity(this.xDocument, columnName, dataType, dataOptions);

            if (element == null) return false;
            Columns.Add(new Column(columnName, dataType, dataOptions, element));
            return true;
        }

        public Column getColumn(string tableName)
        {
            //return xDocToColumn.tryParse(new TableDetails().getXmlColumn(tableName, xDocument));
            return Columns.First(Col => Col.columnName == tableName);
        }

        public bool RemoveColumn(string columnName)
        {
            Column column = Columns.First(col => col.columnName == columnName);
            if (column == null) return false;

            column.Remove();
            Columns.Remove(column);

            return true;

        }

        public string[] getColumnNames()
        {
            return new TableDetails().getColumnesName(xDocument);
        }

        public Entity getEntities()
        {
            return entity;
        }

        public void Save()
        {
            new TableIO().Save(tableName, path, xDocument);
        }


        public void addLine()
        {
            XElement xLine = new TableDetails().addLine(xDocument);
            Lines.Add(new Line(Columns.ToArray(), xLine));

        }

        public Row getRow(object RowValue, string columnName)
        {
            try
            {
                Line lin = Lines.First(line => line.getRow(columnName.ToString()).get() == RowValue.ToString());
                return lin.Rows.First(row => row.parentColumn.columnName == columnName);
            } catch
            {
                return null;
            }

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
                Columns.Add(xDocToColumn.tryParse(el));
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
    }
}
