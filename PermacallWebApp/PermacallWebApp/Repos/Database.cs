using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace PermacallWebApp.Repos
{
    public class Database
    {
        public static string ConnectionString
        {
            get
            {

                
                string connection;
                switch ("Laptop")
                {

#pragma warning disable 162
                    case "Laptop":
                        connection = "DevelopmentLaptop";
                        break;
                    default:
                        connection = "DevelopmentLaptop";
                        break;
#pragma warning restore 162
                }
                return LoginData.DatabaseString;
            }
        }
    }
}