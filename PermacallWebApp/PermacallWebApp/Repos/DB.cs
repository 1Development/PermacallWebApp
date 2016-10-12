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
        public static IRepository Repo
        {
            get
            {
                if (Repo != null) Repo = new MySQLRepo();
                return Repo;
            }
            private set { Repo = value; }
        }

        public static string ConnectionString { get { return SecureData.DatabaseString; } }
    }
}