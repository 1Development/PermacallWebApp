using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallTools.Controllers;
using PermacallTools.Models.Seeker;

namespace PermacallTools.Repos
{
    public class SeekerRepo
    {
        public static bool CreateGame(string GameCode, string androidID)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"INSERT INTO GAME(GameCode, host) VALUES(@gamecode, @host)";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));
                        cmd.Parameters.Add(new MySqlParameter("host", androidID));

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

        public static bool GameCodeExist(string GameCode)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT GameCode,started,host FROM GAME WHERE UPPER(GAMECODE) = @gamecode ORDER BY ID DESC LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));

                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (DateTime.Parse(reader["started"].ToString()).AddMinutes(60) > DateTime.Now)
                                    {
                                        return true;
                                    }
                                }

                            }
                        }

                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }

        public static bool JoinGame(string GameCode, string PlayerName, string androidID)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"INSERT INTO PLAYER(playername, gameCode, uniqueID) VALUES (@plrName, @gamecode, @androidid)";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("plrName", PlayerName));
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));
                        cmd.Parameters.Add(new MySqlParameter("androidid", androidID));

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

        public static bool isHost(string GameCode, string androidID)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT started,host FROM GAME WHERE UPPER(GAMECODE) = @gamecode ORDER BY ID DESC LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));

                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (DateTime.Parse(reader["started"].ToString()).AddMinutes(60) > DateTime.Now)
                                    {
                                        if (reader["host"].ToString() == androidID)
                                            return true;
                                    }
                                }

                            }
                        }

                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }
        public static bool isStarted(string GameCode)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT started,hasStarted FROM GAME WHERE UPPER(GAMECODE) = @gamecode ORDER BY ID DESC LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));

                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (DateTime.Parse(reader["started"].ToString()).AddMinutes(60) > DateTime.Now)
                                    {
                                        if(reader["hasStarted"].ToString() =="1")
                                            return true;
                                    }
                                }

                            }
                        }

                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }

        public static bool StartGame(string GameCode, string androidID)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE GAME SET hasstarted = 1 WHERE GameCode = @gamecode AND host = @hostcode";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gamecode", GameCode.ToUpper()));
                        cmd.Parameters.Add(new MySqlParameter("hostcode", androidID));

                        {
                            if (cmd.ExecuteNonQuery() > 0) return true;
                        }

                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }

        public static bool PlusScore(string GameCode, string name, int scoreCount)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE PLAYER SET score = score + @scorecount WHERE gamecode = @GameCode AND uniqueID = @androidID";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("scorecount", scoreCount));
                        cmd.Parameters.Add(new MySqlParameter("GameCode", GameCode.ToUpper()));
                        cmd.Parameters.Add(new MySqlParameter("Name", name));


                        if (cmd.ExecuteNonQuery() > 0) return true;


                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }
        public static bool SetFound(string GameCode, string androidID)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"UPDATE PLAYER SET found = 1 WHERE gamecode = @GameCode AND uniqueID = @androidID";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("GameCode", GameCode.ToUpper()));
                        cmd.Parameters.Add(new MySqlParameter("androidID", androidID));


                        if (cmd.ExecuteNonQuery() > 0) return true;


                    }
                }

            }
            catch (MySqlException)
            { }
            return false;
        }

        public static List<Score> GetScores(string GameCode)
        {
            List<Score> scores = new List<Score>();
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    string getSaltSQL = @"SELECT DISTINCT playername, score, found FROM PLAYER WHERE GameCode = @gameCode ORDER BY SCORE";
                    using (MySqlCommand cmd = new MySqlCommand(getSaltSQL, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("gameCode", GameCode.ToUpper()));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bool f = false;
                                if (reader["found"].ToString() == "1") f = true;

                                scores.Add(new Score()
                                {
                                    Name = reader["playername"].ToString(),
                                    Points = reader["score"].ToInt(),
                                    found = f
                                });

                            }
                        }

                    }
                }

            }
            catch (MySqlException)
            {
                return null;
            }
            return scores;
        }

    }
}