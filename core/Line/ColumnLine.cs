using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB.core
{
    public class ColumnLine
    {
        public List<Row> Rows { get; private set; } = new List<Row>();

        public void MoveUp()
        {
            if (Rows.Count == 0) return;

            for(int i=1; i < Rows.Count; i++ )
            {
                var tmpRow = Rows[i - 1].get();

                Rows[i - 1].set(Rows[i].get());
                Rows[i].set(tmpRow);
            }
        }

        public void MoveDown()
        {
            if (Rows.Count == 0) return;

            for(int i = Rows.Count - 1; i > 0; i--)
            {
                var tmpRow = Rows[i].get();

                Rows[i].set(Rows[i - 1].get());
                Rows[i - 1].set(tmpRow);
            }
        }

        public void Swap(int oldIndex, int newIndex)
        {
            var tmpRow = Rows[oldIndex].get();

            Rows[oldIndex].set(Rows[newIndex].get());
            Rows[newIndex].set(tmpRow);
        }

        public void Swap(string oldValue, string newValue)
        {
            var tmpRow = Rows.First(el => el.get() == oldValue);

            Rows.First(el => el.get() == oldValue).set(newValue);
            Rows.First(el => el.get() == newValue).set(tmpRow);
        }

        public void SwapLine(ColumnLine target)
        {
            for(int i=0; i < Rows.Count; i++)
            {
                var tmpVal = target.Rows[i].get();

                target.Rows[i].set(Rows[i].get());
                Rows[i].set(tmpVal);
            }
        }

        public void RemoveAllRows()
        {
            foreach(Row row in Rows)
            {
                row.remove();
            }
        }
    }
    
}
