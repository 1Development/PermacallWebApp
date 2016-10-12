using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;

namespace PermacallWebApp.Repos
{
    public class MySQLClass
    {
        private static string ConnectionString { get { return SecureData.DatabaseString; } }

        enum ReturnCode
        {
            Succes,
            GeneralFail,
            NoResults,
            ConnectionError
        }

        static public bool CheckExist(string SQLquery, Dictionary<string, string> parameters)
        {
            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Value);
            }

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
                            if (reader.Read())
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return false;
            }
        }

        static public Dictionary<string, string> GetOneResultQuery(string SQLquery, Dictionary<string, string> parameters)
        {
            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Value);
            }

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
                            if (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    returnDictionary.Add(reader.GetName(i), reader.GetString(i));
                                }
                                return returnDictionary;
                            }
                            return new Dictionary<string, string>() {{"ERROR", "NORESULTS"}};
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return new Dictionary<string, string>() { { "ERROR", "NOCONNECTION" } };
            }
        }

        static public List<Dictionary<string, string>> GetMultipleResultsQuery(string SQLquery, Dictionary<string, string> parameters)
        {
            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Value);
            }

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Dictionary<string, string>> returnList = new List<Dictionary<string, string>>();
                            while (reader.Read())
                            {
                                Dictionary<string, string> thisRow = new Dictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    thisRow.Add(reader.GetName(i), reader.GetString(i));
                                }
                                returnList.Add(thisRow);
                            }
                            return returnList;
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return null;
            }
        }

        static public bool updateQuery(string SQLquery, List<KeyValuePair<string, string>> parameters)
        {
            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Value);
            }

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                        }

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch (MySqlException)
            {
                return false;
            }
        }

        static bool deleteQuery(string SQLquery, List<KeyValuePair<string, string>> parameters)
        {
            return updateQuery(SQLquery, parameters);
        }

        private static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}