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
        private XElement xElement;

        public Column(string columnName, DataType dataType, DataOptions[] dataOptions, XElement element)
        {
            this.columnName = columnName;
            this.dataType = dataType;
            this.dataOptions = dataOptions;
            this.xElement = element;
        }

        public bool EditName(string newName)
        {
            if (xElement.Parent == null) return false;
            XElement EntityElement = xElement.Parent;
            if (EntityElement.Elements().Where(el => el.Attribute("Name").Value == newName).Count() >= 1) return false;

            xElement.Attribute("Name").Value = newName;
            columnName = newName;
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

        public bool MoveDown()
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

            return true;

        }

        public bool MoveUp()
        {
            if (xElement.Parent == null) return false;
            XElement EntityElement = xElement.Parent;

            XElement elementToMove;
            try
            {
                elementToMove = EntityElement.Elements().First(el => el.Attribute("Name").Value == columnName);
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

    }
}
