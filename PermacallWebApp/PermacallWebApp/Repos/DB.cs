using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace PermacallWebApp.Repos
{
    public class DB
    {
        private static IDatabaseRepo mainDB;
        public static IDatabaseRepo MainDB
        {
            get
            {
                if (mainDB == null) mainDB = new MySQLRepo(SecureData.DatabaseString);
                return mainDB;
            }
            private set { mainDB = value; }
        }
    }
}