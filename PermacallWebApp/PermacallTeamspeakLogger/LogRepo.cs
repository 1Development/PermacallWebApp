using System.Collections.Generic;
using System.Net.Sockets;
using PCDataDLL;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallTeamspeakLogger
{
    public class LogRepo
    {
        public static bool Log()
        {
            ListResponse<ClientListEntry> clientList;
            try
            {
                using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
                {
                    queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                    queryRunner.SelectVirtualServerById(1);
                    queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                    {
                        // REAL EXCECUTED CODE
                        clientList = queryRunner.GetClientList();
                    }
                    queryRunner.Logout();
                }
            }
            catch (SocketException)
            {
                clientList = new ListResponse<ClientListEntry>();
            }


            List<string> queries = new List<string>();
            List<Dictionary<string, object>> parameterList = new List<Dictionary<string, object>>();
            foreach (var client in clientList)
            {
                queries.Add("INSERT INTO TSLoggedUser(TSChannelID, TSUserID) VALUES(?, ?)");
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"channelID", client.ChannelId.ToString()},
                    {"userID", client.ClientDatabaseId.ToString()}
                };
                parameterList.Add(parameters);
            }

            var result = DB.MainDB.InsertMultiQuery(queries, parameterList);

            return result;
        }
    }
}