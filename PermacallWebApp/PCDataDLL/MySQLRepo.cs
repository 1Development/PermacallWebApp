using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PCDataDLL
{
    public class MySQLRepo
    {
        public string ConnectionString { private get; set; }

        public MySQLRepo(string connectionString)
        {
            this.ConnectionString = connectionString;
            //dit is ExecutionEngineException test;
        }

        public bool CheckExist(string SQLquery, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Key);
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

        public DBResult GetOneResultQuery(string SQLquery, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Key);
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
                                    if (!reader.IsDBNull(i))
                                        returnDictionary.Add(reader.GetName(i), reader.GetString(i));
                                }
                                return new DBResult(returnDictionary);
                            }
                            return new DBResult(new Dictionary<string, string>());
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return null;
            }
        }

        public List<DBResult> GetMultipleResultsQuery(string SQLquery, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Key);
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
                            List<DBResult> returnList = new List<DBResult>();
                            while (reader.Read())
                            {
                                Dictionary<string, string> thisRow = new Dictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (!reader.IsDBNull(i))
                                        thisRow.Add(reader.GetName(i), reader.GetString(i));
                                }
                                returnList.Add(new DBResult(thisRow));
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

        public bool UpdateQuery(string SQLquery, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            string sql = SQLquery;
            foreach (var parameter in parameters)
            {
                sql = ReplaceFirst(sql, "?", "@" + parameter.Key);
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
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool UpdateMultiQuery(List<string> SQLquerys, List<Dictionary<string, object>> parametersList)
        {
            if (parametersList == null)
                parametersList = new List<Dictionary<string, object>>();

            string sql = "";
            Dictionary<string, object> parameters;
            MySqlTransaction transaction = null;
            

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    for (int i = 0; i < SQLquerys.Count; i++)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn,transaction))
                        {
                            sql = SQLquerys[i];
                            parameters = parametersList[i];

                            foreach (var parameter in parameters)
                            {
                                sql = ReplaceFirst(sql, "?", "@" + parameter.Key);
                            }

                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                            }

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }

                    transaction.Commit();

                }
                return true;

            }
            catch (MySqlException e)
            {
                transaction?.Rollback();

                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteQuery(string SQLquery, Dictionary<string, object> parameters)
        {
            return UpdateQuery(SQLquery, parameters);
        }
        public bool DeleteMultiQuery(List<string> SQLquerys, List<Dictionary<string, object>> parametersList)
        {
            return UpdateMultiQuery(SQLquerys, parametersList);
        }
        public bool InsertQuery(string SQLquery, Dictionary<string, object> parameters)
        {
            return UpdateQuery(SQLquery, parameters);
        }
        public bool InsertMultiQuery(List<string> SQLquerys, List<Dictionary<string, object>> parametersList)
        {
            return UpdateMultiQuery(SQLquerys, parametersList);
        }

        private string ReplaceFirst(string text, string search, string replace)
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