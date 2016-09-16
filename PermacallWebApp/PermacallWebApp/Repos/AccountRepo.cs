using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Repos
{
    public class AccountRepo
    {
        public static Tuple<bool,string> GetSalt(string username)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = @username";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("username", username.ToLower()));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) return new Tuple<bool, string>(true, reader["SALT"].ToString());
                            else return new Tuple<bool, string>(false, "NO_SALT");
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return new Tuple<bool, string>(false, "NOCONNECTION");
            }
        }

        public static bool CheckAvailable(string username)
        {
            var result = GetSalt(username);
            if (!result.Item1 && result.Item2 == "NO_SALT") return true;
            return false;
        }

        public static Tuple<bool,string> ValidateCredentials(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = @username AND PASSWORD = @password";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("username", username.ToLower()));
                        cmd.Parameters.Add(new MySqlParameter("password", password));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) return new Tuple<bool, string>(true, reader["ID"].ToString());
                            else return new Tuple<bool, string>(false,"NOTCORRECT");
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return new Tuple<bool, string>(false, "NOCONNECTION");
            }
        }

        public static bool SetSessionKey(string username, string sessionKey)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE ACCOUNT SET SESSIONKEY=@sessionkey WHERE LOWER(USERNAME) = @username";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("sessionkey", sessionKey));
                        cmd.Parameters.Add(new MySqlParameter("username", username.ToLower()));

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }

            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public static User GetUser(string sessionKey)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT ID, OPERATORCOUNT, USERNAME FROM ACCOUNT WHERE SESSIONKEY = @sessionKey";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("sessionKey", sessionKey));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return new User(reader["ID"].ToInt(),
                                    reader["OPERATORCOUNT"].ToInt(),
                                    reader["USERNAME"].ToString());
                            else return new User(0, 0, "NOSESSION");
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return new User(-1, 0, "NOCONNECTION");
            }
        }

        public static bool InsertNewAccount(string username, string password, string salt)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"INSERT INTO ACCOUNT(USERNAME, PASSWORD, SALT) VALUES (@username, @password, @salt)";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("username", username));
                        cmd.Parameters.Add(new MySqlParameter("password", password));
                        cmd.Parameters.Add(new MySqlParameter("salt", salt));

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }

            }
            catch (MySqlException)
            {
                return false;
            }
        }


    }
}