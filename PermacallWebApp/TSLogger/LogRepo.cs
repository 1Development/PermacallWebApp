using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
                    {
                        queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                        queryRunner.SelectVirtualServerById(1);
                        queryRunner.UpdateCurrentQueryClient(new ClientModification {Nickname = "PermacallWebApp"});

                        {
                            // REAL EXCECUTED CODE
                            channelList = queryRunner.GetChannelList();
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

                foreach (var channel in channelList)
                {
                    queries.Add("INSERT INTO tslog_channel(channelID, channelName) VALUES(?, ?) ON DUPLICATE KEY UPDATE channelName=?");
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"channelID", channel.ChannelId},
                        {"channelName", channel.Name},
                        {"channelName2", channel.Name}
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
    }
}