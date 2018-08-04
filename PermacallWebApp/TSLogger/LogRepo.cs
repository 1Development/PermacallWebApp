using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using PCAuthLib;
using PCDataDLL;
using TS3QueryLib.Core;
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
                        queryRunner.UpdateCurrentQueryClient(new ClientModification {Nickname = "PermacallWebApp"});

                        {
                            // REAL EXCECUTED CODE
                            channelList = queryRunner.GetChannelList(true);
                            clientList = queryRunner.GetClientList();
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
            File.AppendAllLines("C:\\www\\TSLoggerLog.txt", new []{ sb.ToString() } );
        }

        public static void OrderGamingChannels()
        {
            ListResponse<ChannelListEntry> channelList;
            List<uint> popularityRanks = GetGamingChannelPopularities();
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

                        uint? currentOrder = popularityRanks[0];
                        foreach (var chnl in popularityRanks)
                        {
                            if (channelList.Values.Exists(x => x.ChannelId == chnl))
                            {
                                queryRunner.EditChannel(chnl, new ChannelModification() { ChannelOrder = currentOrder });
                                currentOrder = chnl;
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
    }

}