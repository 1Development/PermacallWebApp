using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCDataDLL
{
    public class DB
    {
        private static MySQLRepo mainDB;
        private static MySQLRepo portFolioDB;
        private static MySQLRepo mpInc;
        public static MySQLRepo MainDB
        {
            get
            {
                if (mainDB == null) mainDB = new MySQLRepo(SecureData.PCDBString);
                return mainDB;
            }
            private set { mainDB = value; }
        }

        public static MySQLRepo PFDB
        {
            get
            {
                if (portFolioDB == null) portFolioDB = new MySQLRepo(SecureData.PFDBString);
                return portFolioDB;
            }
            private set { portFolioDB = value; }
        }
    }
}
