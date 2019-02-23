using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using PCAuthLib;
using PCDataDLL;
using TS3QueryLib.Core;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;

namespace TSLogger
{
    public class LogRepo
    {

        public static bool Log()
        {
            WriteToFile("Starting Logging");
            try
            {
                ListResponse<ChannelListEntry> channelList;
                ListResponse<ClientListEntry> clientList;
                try
                {
                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("192.168.0.220", 10011)))
                    {
                        queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                        queryRunner.SelectVirtualServerById(1);
                        queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                        {
                            // REAL EXCECUTED CODE
                            channelList = queryRunner.GetChannelList(true);
                            clientList = queryRunner.GetClientList(true);

                            //REMOVE AFK USERS FROM NON-AFK
                            foreach (var client in clientList)
                            {
                                if (client.ServerGroups.Contains(13) &&
                                    client.ClientIdleDuration != null)
                                {
                                    if (((TimeSpan)client.ClientIdleDuration).TotalHours > 4)
                                    {
                                        queryRunner.DeleteClientFromServerGroup(13, client.ClientDatabaseId);
                                        queryRunner.SendTextMessage(MessageTarget.Client, client.ClientId,
                                            "NON-AFK is not meant for AFK users, and since you have been idle for over 4 hours you have been moved to AFK");
                                    }
                                    else if (((TimeSpan)client.ClientIdleDuration).TotalHours > 3)
                                        queryRunner.SendTextMessage(MessageTarget.Client, client.ClientId,
                                            "NON-AFK is not meant for AFK users, and you have been idle for over 3 hours now. If you are truly not AFK take some action the server will see(like muting/unmuting, moving, AFK/unAFK)");

                                }
                            }
                        }
                        queryRunner.Logout();
                    }
                }
                catch (SocketException)
                {
                    channelList = new ListResponse<ChannelListEntry>();
                    clientList = new ListResponse<ClientListEntry>();
                }


                List<string> queries = new List<string>();
                List<Dictionary<string, object>> parameterList = new List<Dictionary<string, object>>();
                
                foreach (var client in clientList)
                {
                    queries.Add("INSERT INTO tslog_loggeduser(TSChannelID, TSUserID) VALUES(?, ?)");
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"channelID", client.ChannelId.ToString()},
                        {"userID", client.ClientDatabaseId.ToString()}
                    };
                    parameterList.Add(parameters);

                    queries.Add("INSERT INTO tslog_TSUser(TSUserID, TSUsername) VALUES(?, ?) ON DUPLICATE KEY UPDATE TSUsername=?");
                    parameters = new Dictionary<string, object>()
                    {
                        {"TSUserID", client.ClientDatabaseId.ToString()},
                        {"TSUsername", client.Nickname},
                        {"TSUsername2", client.Nickname}
                    };
                    parameterList.Add(parameters);
                }


                queries.Add("UPDATE tslog_channel SET doesExist=0");
                parameterList.Add(new Dictionary<string, object>());

                foreach (var channel in channelList)
                {
                    queries.Add("INSERT INTO tslog_channel(channelID, channelName, parentChannelID) VALUES(?, ?, ?) ON DUPLICATE KEY UPDATE channelName=?, doesExist=1, parentChannelID=?");
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"channelID", channel.ChannelId},
                        {"channelName", channel.Name},
                        {"parentChannelID", channel.ParentChannelId},
                        {"channelName2", channel.Name},
                        {"parentChannelID2", channel.ParentChannelId}
                    };
                    parameterList.Add(parameters);
                }

                
                var result = DB.MainDB.InsertMultiQuery(queries, parameterList);



                WriteToFile("Done with Logging");
                return result;
            }
            catch (Exception e)
            {
                WriteToFile(e.ToString());
            }
            return false;
        }

        public static void WriteToFile(string toWrite)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.Now.ToLongTimeString());
            sb.Append("] ");
            sb.Append(toWrite);
            File.AppendAllLines("C:\\www\\TSLoggerLog.txt", new[] { sb.ToString() });
        }

        public static void OrderGamingChannels()
        {
            ListResponse<ChannelListEntry> channelList;
            List<ChannelScore> popularityRanks = GetDeHistoricGamingChannelPopularities();
            if (popularityRanks.Count <= 1) return;
            try
            {
                using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("192.168.0.220", 10011)))
                {
                    queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                    queryRunner.SelectVirtualServerById(1);
                    queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                    {

                        channelList = queryRunner.GetChannelList(true);

                        uint? currentOrder = 0; //popularityRanks[0].Id;
                        foreach (var chnl in popularityRanks)
                        {
                            if (channelList.Values.Exists(x => x.Name == chnl.Name))
                            {
                                queryRunner.EditChannel(chnl.Id, new ChannelModification() { ChannelOrder = currentOrder, Description = "Channel Popularity Score: " + Math.Round(chnl.Score) });
                                currentOrder = chnl.Id;
                            }
                        }
                        queryRunner.EditChannel(8, new ChannelModification() { ChannelOrder = currentOrder }); // SET RANDOMGAME LAST
                    }
                    queryRunner.Logout();
                }
            }
            catch (SocketException)
            {
            }
        }


        public static List<uint> GetGamingChannelPopularities()
        {
            List<uint> rankResults = new List<uint>();
            string sql =
                "SELECT channel.channelID FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.Timestamp > DATE_SUB(CURDATE(), INTERVAL 30 DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID WHERE channel.parentChannelID = 2 and channel.doesExist = 1 GROUP BY channel.channelID ORDER BY COUNT(channel.channelID) DESC";
            List<DBResult> results = DB.MainDB.GetMultipleResultsQuery(sql, null);
            foreach (var dbResult in results)
            {
                rankResults.Add(dbResult.Get("channelID").ToUInt());
            }
            return rankResults;
        }

        public static List<ChannelScore> GetDeHistoricGamingChannelPopularities()
        {
            List<ChannelScore> rankResults = new List<ChannelScore>();
            Dictionary<uint, ChannelScore> scores = new Dictionary<uint, ChannelScore>();

            string sql1 =
                "select channel.channelID, channel.channelName, COUNT(channel.channelName) / 7 * 100 as 'score' FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.Timestamp > DATE_SUB(CURDATE(), INTERVAL 7 DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID WHERE (channel.parentChannelID = 2 and channel.doesExist = 1) or channel.parentChannelID in (select tc.channelId from tslog_channel tc where tc.parentChannelID = 2) GROUP BY channel.channelName having COUNT(channel.channelName) > 2 ORDER BY COUNT(channel.channelName) desc";
            string sql2 =
                "select channel.channelID, channel.channelName, COUNT(channel.channelName)/300 * 100 as 'score' FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.Timestamp > DATE_SUB(CURDATE(), INTERVAL 30 DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID WHERE (channel.parentChannelID = 2 and channel.doesExist = 1) or channel.parentChannelID in (select tc.channelId from tslog_channel tc where tc.parentChannelID = 2) GROUP BY channel.channelName having COUNT(channel.channelName) > 2 ORDER BY COUNT(channel.channelName) desc";
            string sql3 =
                "select channel.channelID, channel.channelName, COUNT(channel.channelName)/18000 * 100 as 'score' FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.Timestamp > DATE_SUB(CURDATE(), INTERVAL 180 DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID WHERE (channel.parentChannelID = 2 and channel.doesExist = 1) or channel.parentChannelID in (select tc.channelId from tslog_channel tc where tc.parentChannelID = 2) GROUP BY channel.channelName having COUNT(channel.channelName) > 2 ORDER BY COUNT(channel.channelName) desc";
            List<DBResult> results1 = DB.MainDB.GetMultipleResultsQuery(sql1, null);
            List<DBResult> results2 = DB.MainDB.GetMultipleResultsQuery(sql2, null);
            List<DBResult> results3 = DB.MainDB.GetMultipleResultsQuery(sql3, null);

            if (results1 != null)
            {
                foreach (var dbResult in results1)
                {
                    if (!scores.ContainsKey(dbResult.Get("channelID").ToUInt()))
                        scores.Add(dbResult.Get("channelID").ToUInt(),
                            new ChannelScore(dbResult.Get("channelName"), dbResult.Get("channelID").ToUInt()));
                    scores[dbResult.Get("channelID").ToUInt()].Score += dbResult.Get("score").ToDouble();
                }
            }

            if (results2 != null)
            {
                foreach (var dbResult in results2)
                {
                    if (!scores.ContainsKey(dbResult.Get("channelID").ToUInt()))
                        scores.Add(dbResult.Get("channelID").ToUInt(),
                            new ChannelScore(dbResult.Get("channelName"), dbResult.Get("channelID").ToUInt()));
                    scores[dbResult.Get("channelID").ToUInt()].Score += dbResult.Get("score").ToDouble();
                }
            }

            if (results3 != null)
            {
                foreach (var dbResult in results3)
                {
                    if (!scores.ContainsKey(dbResult.Get("channelID").ToUInt()))
                        scores.Add(dbResult.Get("channelID").ToUInt(),
                            new ChannelScore(dbResult.Get("channelName"), dbResult.Get("channelID").ToUInt()));
                    scores[dbResult.Get("channelID").ToUInt()].Score += dbResult.Get("score").ToDouble();
                }
            }

            var orderedList = scores.OrderByDescending(x => x.Value.Score).ToList();

            foreach (var keyValuePair in orderedList)
            {
                rankResults.Add(keyValuePair.Value);
            }

            return rankResults;
        }
    }

}