using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1_DataBase_1
{
    public class Relationship
    {
        string Primary_Table, Foreign_Table, Column;

        public Relationship()
        {
            Primary_Table = null;
            Foreign_Table = null;
            Column = null;
        }

        public void SetPrimary(string primary)
        {
            Primary_Table = primary;
        }

        public void SetForeign(string foreign)
        {
            Foreign_Table = foreign;
        }

        public void SetColumn(string column)
        {
            this.Column = column;
        }

        public string GetPrimary()
        {
            return Primary_Table;
        }

        public string GetForeign()
        {
            return Foreign_Table;
        }

        public string GetColumn()
        {
            return Column;
        }
    }
}
