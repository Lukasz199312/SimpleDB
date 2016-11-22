using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDB.core.TableManagement;
using System.Xml.Linq;

namespace SimpleDB.core
{
    public class Entity
    {
        private XElement EntityElement;

        public Entity(XElement _Entity)
        {
            this.EntityElement = _Entity;
        }

        public bool EditName(string newName, string oldName)
        {
            try
            {
                if (EntityElement.Elements().First(el => el.Attribute("Name").Value == newName) != null) return false;
                XElement xElement = EntityElement.Elements().First(el => el.Attribute("Name").Value == oldName);
                xElement.Attribute("Name").Value = newName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditDataType(string EntityName, DataType newDataType)
        {
            try
            {
                XElement xElement = EntityElement.Elements().First(el => el.Attribute("Name").Value == EntityName);
                xElement.Attribute("Name").Value = newDataType.ToString();

                return true;
            } catch
            {
                return false;
            }
        }

        public void RemoveDataOption(string entityName, DataOptions[] newDataOptions, DataOptions[] oldDataOptions)
        {
            IEnumerable<XAttribute> attributes = EntityElement.Elements().First(el => el.Attribute("Name").Value == entityName).Attributes();
        }

        public bool RemoveElement(string Name)
        {
           try
            {
                EntityElement.Elements().First(el => el.Attribute("Name").Value == Name).Remove();
            } catch
            {
                return false;
            }

            return true;
        }

        public bool MoveDown(string Name)
        {
            if (EntityElement.Elements().ToList<XElement>()[0].Attribute("Name").Value == Name) return false;
            XElement elementToMove;

            try {
                elementToMove = EntityElement.Elements().First(el => el.Attribute("Name").Value == Name);
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

        public bool MoveUp(string Name)
        {
            XElement elementToMove;
            try
            {
                elementToMove = EntityElement.Elements().First(el => el.Attribute("Name").Value == Name);
            } catch
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
