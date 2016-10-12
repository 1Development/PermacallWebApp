using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Repos
{
    public class TeamspeakUserRepo
    {
        public static List<TSUser> GetTeamspeakUsers(int accountID)
        {
            List<TSUser> returnList = new List<TSUser>();
            try
            {
                using (var conn = new MySqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT TEAMSPEAKDBID, NICKNAME FROM TEAMSPEAKUSER WHERE ACCOUNTID = @accountid AND ENABLED = 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("accountid", accountID));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                returnList.Add(
                                    new TSUser(
                                        reader["TEAMSPEAKDBID"].ToString(),
                                        reader["NICKNAME"].ToString(),
                                        accountID));
                            }
                        }
                    }
                }
                return returnList;

            }
            catch (MySqlException)
            {
                return new List<TSUser>();
            }
        }
        public static bool TSUserAvailable(string DBID)
        {
            List<TSUser> returnList = new List<TSUser>();
            try
            {
                using (var conn = new MySqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT TEAMSPEAKDBID, NICKNAME FROM TEAMSPEAKUSER WHERE TEAMSPEAKDBID = @teamspeakid AND ENABLED = 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("teamspeakid", DBID));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                }

            }
            catch (MySqlException)
            {
                return false;
            }
        }


        public static List<TSUser> AddTeamspeakUserToAccount(TSUser toAddUser)
        {
            List<TSUser> returnList = new List<TSUser>();
            try
            {
                using (var conn = new MySqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"INSERT INTO TEAMSPEAKUSER(TEAMSPEAKDBID, ACCOUNTID, NICKNAME) VALUES(@teamspeakid, @accountid, @nickname)";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("teamspeakid", toAddUser.TeamspeakDBID));
                        cmd.Parameters.Add(new MySqlParameter("accountid", toAddUser.AccountID));
                        cmd.Parameters.Add(new MySqlParameter("nickname", toAddUser.NickName));

                        cmd.ExecuteNonQuery();
                    }
                }
                return returnList;

            }
            catch (MySqlException)
            {
                return new List<TSUser>();
            }
        }

        public static List<TSUser> EditTSUser(string teamspeakid, TSUser editResult)
        {
            List<TSUser> returnList = new List<TSUser>();
            try
            {
                using (var conn = new MySqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE TEAMSPEAKUSER SET TEAMSPEAKDBID = @newteamspeakid, ACCOUNTID = @accountid, NICKNAME = @nickname WHERE ENABLED = @enabled AND TEAMSPEAKDBID = @teamspeakid";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("newteamspeakid", editResult.TeamspeakDBID));
                        cmd.Parameters.Add(new MySqlParameter("accountid", editResult.AccountID));
                        cmd.Parameters.Add(new MySqlParameter("nickname", editResult.NickName));
                        cmd.Parameters.Add(new MySqlParameter("enabled", 1));
                        cmd.Parameters.Add(new MySqlParameter("teamspeakid", teamspeakid));


                        cmd.ExecuteNonQuery();
                    }
                }
                return returnList;

            }
            catch (MySqlException)
            {
                return new List<TSUser>();
            }
        }

        public static List<TSUser> DisableTSUser(string teamspeakid)
        {
            List<TSUser> returnList = new List<TSUser>();
            try
            {
                using (var conn = new MySqlConnection(DB.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE TEAMSPEAKUSER SET ENABLED = 0 WHERE TEAMSPEAKDBID = @teamspeakid";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("teamspeakid", teamspeakid));


                        cmd.ExecuteNonQuery();
                    }
                }
                return returnList;

            }
            catch (MySqlException)
            {
                return new List<TSUser>();
            }
        }
    }
}