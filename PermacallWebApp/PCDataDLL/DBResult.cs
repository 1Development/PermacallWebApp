using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCDataDLL
{
    public class DBResult
    {
        private Dictionary<string, string> resultRow;

        public DBResult(Dictionary<string, string> results)
        {
            resultRow = results;
        }

        public string Get(string column)
        {
            if (resultRow.ContainsKey(column))
                return resultRow[column];
            if (resultRow.ContainsKey(column.ToUpper()))
                return resultRow[column.ToUpper()];
            return null;
        }

        public bool Exists()
        {
            return resultRow.Keys.Count > 0;
        }

        public int KeyCount()
        {
            return resultRow.Keys.Count;
        }

        public Dictionary<string, string>.KeyCollection GetKeyCollection()
        {
            return resultRow.Keys;
        }
    }
}
