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
                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("192.168.0.230", 10011)))
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
                using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("192.168.0.230", 10011)))
                {
                    queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                    queryRunner.SelectVirtualServerById(1);
                    queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

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

                    queryRunner.Logout();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
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

            string sql =
                "select channel.channelId, channel.channelName, IFNULL(datatable.score, 0) as score from tslog_channel channel left outer join ( select channelID, round(sum(t.age_weighted)) as score from ( select t2.*, (GREATEST((1-age*age*0.0001), 0) + GREATEST((3-age*age*0.0003), 0) + GREATEST((7-age*age*0.15), 0)) as age_weighted from ( select log.id, channel.channelName, channel.channelID, DATEDIFF(curdate(),	log.`Timestamp`) as age from tslog_tsuser tsuser inner join tslog_loggeduser log on log.TSUserID = tsuser.TSUserID and tsuser.TSUserID not in (1, 10) right outer join tslog_channel channel on log.TSChannelID = channel.channelID where channel.parentChannelID = 2 and channel.doesExist and log.Timestamp > DATE_SUB(curdate(), interval 200 day) order by log.ID desc ) t2 ) t group by t.channelID order by score desc ) datatable on datatable.channelID = channel.channelID where channel.parentChannelID = 2 and channel.doesExist order by score desc";

            List<DBResult> results = DB.MainDB.GetMultipleResultsQuery(sql, null);

            if (results != null)
            {
                foreach (var dbResult in results)
                {
                    if (!scores.ContainsKey(dbResult.Get("channelId").ToUInt()))
                        scores.Add(dbResult.Get("channelId").ToUInt(),
                            new ChannelScore(dbResult.Get("channelName"), dbResult.Get("channelId").ToUInt()));
                    scores[dbResult.Get("channelId").ToUInt()].Score += dbResult.Get("score").ToDouble();
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