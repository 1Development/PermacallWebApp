using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;

namespace PermacallWebApp.Repos
{
    public class Repository
    {

        enum ReturnCode
        {
            Succes,
            GeneralFail,
            NoResults,
            ConnectionError
        }

        public static  Dictionary<string,string> GetOneResult(string SQLquery, List<KeyValuePair<string,string>> parameters)
        {
            string sql = SQLquery;
            for (int i = 0; i < parameters.Count; i++)
            {
                sql = sql.Replace("?", "@" + parameters[i].Key);
            }

            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            cmd.Parameters.Add(new MySqlParameter(parameters[i].Key, parameters[i].Value));
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                            //    return
                            } 
                            //else return new Tuple<bool, string>(false, "NO_RETURN");
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                
            }

            return new Dictionary<string, string>()
                {
                    {"test", "test2"}
                };
        }
    }
}