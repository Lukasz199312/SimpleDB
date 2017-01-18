using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDB.core.TableManagement;
using System.Xml.Linq;

namespace SimpleDB.core
{
    public class Column
    {
        public string columnName { get; private set; }
        public DataType dataType { get; private set; }
        public DataOptions[] dataOptions { get; private set; }
        public string Optional;
        public Table parentTable;
        public ColumnLine columnLine { get; private set; }

        private XElement xElement;
        //private List<Reference> References = new List<Reference>();
        private Reference reference;

        public Column(string columnName, DataType dataType, DataOptions[] dataOptions, XElement element, Table parent)
        {
            this.columnName = columnName;
            this.dataType = dataType;
            this.dataOptions = dataOptions;
            this.xElement = element;
            this.parentTable = parent;

            columnLine = new ColumnLine();
        }


        public bool setOptional(string param)
        {
            if (xElement.Parent == null) return false;
            XElement EntityElement = xElement.Parent;

            try
            {
                EntityElement.Elements().Where(el => el.Attribute("Name").Value == columnName).First().Attribute("Optional").Value = param;
            }
            catch {
                EntityElement.Elements().Where(el => el.Attribute("Name").Value == columnName).First().Add(new XAttribute("Optional", param));
            }

            Optional = param;
            return true;
        }


        public string getOptional()
        {
            return Optional;
        }

        public bool EditName(string newName)
        {
            if (xElement.Parent == null) return false;
            string oldName = columnName;
            XElement EntityElement = xElement.Parent;
            if (EntityElement.Elements().Where(el => el.Attribute("Name").Value == newName).Count() >= 1) return false;

            xElement.Attribute("Name").Value = newName;
            columnName = newName;

            if (reference != null) ReferenceHelper.editName(reference, oldName, newName);

            return true;
        }


        public void EditDataType(DataType newDataType)
        {
            xElement.Attribute("DataType").Value = newDataType.ToString();
            dataType = newDataType;
        }

        //Sprawdzic popawnosc dzialania
        public bool RemoveDataOption(DataOptions dataOption)
        {
            var xAttribute = xElement.Attributes().First(attribute => attribute.Name == dataOption.ToString());
            if (xAttribute == null) return false;

            dataOptions.ToList().Remove(dataOption);
            xAttribute.Remove();
            return true;
        }

        public void Remove()
        {
            xElement.Remove();
        }

        public void RemoveReference()
        {
            foreach(DataOptions option in dataOptions)
            {
                if(option == DataOptions.FOREIGN_KEY)
                {
                    reference.Remove();
                    RemoveDataOption(DataOptions.FOREIGN_KEY);
                    TableList.getTableList().getTable(reference.getTableName()).getColumn(reference.getColumnName()).reference.Remove();
                    TableList.getTableList().getTable(reference.getTableName()).getColumn(reference.getColumnName()).RemoveDataOption(DataOptions.FOREIGN_KEY);
                }
            }
        }

        public bool MoveUp()
        {
            if (xElement.Parent == null) return false;
            XElement EntityElement = xElement.Parent;

            if (EntityElement.Elements().ToList<XElement>()[0].Attribute("Name").Value == columnName) return false;
            XElement elementToMove;

            try
            {
                elementToMove = EntityElement.Elements().First(el => el.Attribute("Name").Value == columnName);
            }
            catch
            {
                return false;
            }

            int count = elementToMove.ElementsBeforeSelf().Count();
            elementToMove.Remove();

            EntityElement.Elements().ToList<XElement>()[count - 1].AddBeforeSelf(elementToMove);

            var index = parentTable.getColumns().ToList<Column>().IndexOf(this);

            columnLine.SwapLine(parentTable.getColumns().ToList<Column>()[index - 1].columnLine);
            //columnLine.MoveUp();

            return true;

        }

        public bool addRelationship(Table targetTable, string columnName)
        {
            try
            {
                Column referenceColumn = targetTable.getColumn(columnName);
                addReferenceElement(targetTable.tableName, columnName);

                targetTable.getColumn(columnName).addReferenceElement(parentTable.tableName, this.columnName);
                //referenceColumn.reference.ForeigenReference = this.reference;
               // this.reference = referenceColumn.reference;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void addReferenceElement(string tableName, string columnName)
        {
            if (xElement.Element("Reference") == null)
                xElement.Add(new XElement("Reference"));

            xElement.Element("Reference").Add(new XAttribute("Table",tableName), new XAttribute("Column",columnName));

            reference = new Reference(xElement.Element("Reference"), this);
            TableList.getTableList().addReferencePrefix(tableName);
            parentTable.ReferenceColumn.Add(columnName);
        }

        internal void InitializeReference()
        {
            try
            {
                reference = new Reference(xElement.Element("Reference"), this);
                parentTable.ReferenceColumn.Add(columnName);
            }
            catch
            {
                return;
            }

        }

        public bool MoveDown()
        {
            if (xElement.Parent == null) return false;
            XElement EntityElement = xElement.Parent;

            XElement elementToMove;
            try
            {
                elementToMove = EntityElement.Elements().First(el => el.Attribute("Name").Value == columnName);

                var index = parentTable.getColumns().ToList<Column>().IndexOf(this);
                columnLine.SwapLine(parentTable.getColumns().ToList<Column>()[index + 1].columnLine);
            }
            catch
            {
                return false;
            }


            if (elementToMove.ElementsAfterSelf().Count() == 0) return false;

            int count = elementToMove.ElementsBeforeSelf().Count();

            elementToMove.Remove();

            EntityElement.Elements().ToList<XElement>()[count].AddAfterSelf(elementToMove);

            return true;
        }

        public void addData(object data)
        {
            if (!checkDataType(data)) return;

           // if(dataType == DataType.NUMBER)

        }

        private bool checkDataType(object data)
        {
            if(dataType == DataType.NUMBER)
            {
                if (dataType.GetType() == typeof(Int32)) return true;
                return false;
            }

            if (dataType.GetType() == typeof(string)) return true;

            return false;
        }


        internal Reference getReference()
        {
            return reference;
        }

        public bool hasReference()
        {
            if (reference != null) return true;
            return false;
        }

    }
}
