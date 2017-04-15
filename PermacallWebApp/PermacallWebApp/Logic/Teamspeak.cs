using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;
using PermacallWebApp.Repos;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Logic
{
    public class Teamspeak
    {
        public static void DisableTeamspeakUser(string teamspeakDBID)
        {
            using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
            {
                queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                queryRunner.SelectVirtualServerById(1);
                queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });
                {
                    TeamspeakUserRepo.DisableTSUser(teamspeakDBID);
                    queryRunner.DeleteClientFromServerGroup(9,
                        Convert.ToUInt32(teamspeakDBID));
                    queryRunner.DeleteClientFromServerGroup(7,
                        Convert.ToUInt32(teamspeakDBID));
                }
                queryRunner.Logout();
            }
        }
    }
}