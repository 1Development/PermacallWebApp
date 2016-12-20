using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Mvc;
using PCDataDLL;
using PermacallWebApp.Logic;
using PermacallWebApp.Models;
using PermacallWebApp.Models.ReturnModels;
using PermacallWebApp.Models.ViewModels;
using PermacallWebApp.Repos;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        [HttpGet]
        public ActionResult Index(int id = -1, int a = 0)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;
            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID <= 0) return RedirectToAction("Index", "Login");

            ManagementModel viewModel = new ManagementModel();

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            currentUser.TSUsers = TeamspeakUserRepo.GetTeamspeakUsers(currentUser.ID);
            viewModel.ToChangeName = "";

            if (id >= 0 && id >= currentUser.TSUsers.Count) return RedirectToAction("AddUser", "Management");
            if (id >= 0 && id <= currentUser.TSUsers.Count)
            {
                if (a == -1)
                {
                    Teamspeak.DisableTeamspeakUser(currentUser.TSUsers[id].TeamspeakDBID);
                    return RedirectToAction("Index", "Management");
                }

                currentUser.TSUsers[id].toEdit = true;
                viewModel.ToChangeName = currentUser.TSUsers[id].NickName;
                viewModel.ToChangeID = id;
            }


            viewModel.CurrentUser = currentUser;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ManagementModel viewModel, int id = -1)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;
            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID <= 0) return RedirectToAction("Index", "Login");

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            currentUser.TSUsers = TeamspeakUserRepo.GetTeamspeakUsers(currentUser.ID);

            if (viewModel.ToChangeName != null && !currentUser.TSUsers.Any(x => x.NickName == viewModel.ToChangeName))
            {
                Repos.TeamspeakUserRepo.UpdateTSUser(currentUser.TSUsers[id].TeamspeakDBID,
                    new TSUser(currentUser.TSUsers[id].TeamspeakDBID, viewModel.ToChangeName, currentUser.ID));
            }

            return RedirectToAction("Index", "Management", new { id = -1 });
        }

        [HttpGet]
        public ActionResult AddUser(int a = 0, int kick = 0)
        {
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, false);
            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID <= 0) return RedirectToAction("Index", "Login");

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.OperatorCount <= 0) return RedirectToAction("Index", "Management");
            currentUser.TSUsers = TeamspeakUserRepo.GetTeamspeakUsers(currentUser.ID);
            if (currentUser.TSUsers.Count >= currentUser.OperatorCount) return RedirectToAction("Index", "Management");

            AddUserModel viewModel = new AddUserModel();
            viewModel.ErrorMessage = "";
            viewModel.ChannelName = currentUser.Username;
            viewModel.Password = Login.GenerateRandomString(currentUser.ID, 6, true, true);

            viewModel.TSName = "";
            viewModel.ChannelEmpty = true;
            viewModel.UserAdded = false;
            viewModel.StartedAdding = false;

            if (a >= 1)
            {
                viewModel.StartedAdding = true;
                using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
                {
                    queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                    queryRunner.SelectVirtualServerById(1);
                    queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                    var channels = queryRunner.GetChannelList();

                    if (channels.Any(x => x.Name == viewModel.ChannelName))
                    {
                        if (a >= 2 || kick == 1)
                        {
                            var channelID = channels.First(x => x.Name == viewModel.ChannelName).ChannelId;
                            var allClients = queryRunner.GetClientList();
                            var targetClient = allClients.Where(entry => entry.ChannelId == channelID);
                            if (targetClient.Any())
                            {
                                if (kick == 1)
                                {
                                    try
                                    {
                                        queryRunner.BanClient(targetClient.First().ClientId,
                                            new TimeSpan(0, 0, 0, 10, 0));
                                    }
                                    catch (FormatException)
                                    {
                                    }

                                    viewModel.ErrorMessage =
                                        "The user has been kicked and temporarilly banned from the server, please try again.";
                                }
                                else
                                {
                                    viewModel.TSName = targetClient.First().Nickname;
                                    viewModel.ChannelEmpty = false;

                                    if (a >= 3)
                                    {
                                        if (currentUser.TSUsers.Any(x => x.TeamspeakDBID == targetClient.First().ClientDatabaseId.ToString()))
                                        {
                                            viewModel.ErrorMessage =
                                                "You already have that teamspeak user bound to your account";
                                        }
                                        else
                                        {
                                            TSUser toAddUser = new TSUser();
                                            toAddUser.TeamspeakDBID = targetClient.First().ClientDatabaseId.ToString();
                                            toAddUser.NickName = targetClient.First().Nickname;
                                            toAddUser.AccountID = currentUser.ID;
                                            if (TeamspeakUserRepo.TSUserAvailable(toAddUser.TeamspeakDBID))
                                            {
                                                TeamspeakUserRepo.AddTeamspeakUserToAccount(toAddUser);
                                                viewModel.UserAdded = true;
                                                queryRunner.AddClientToServerGroup(9, targetClient.First().ClientDatabaseId);
                                                queryRunner.DeleteChannel(channelID, true);
                                            }
                                            else
                                            {
                                                viewModel.ErrorMessage =
                                                "This teamspeak user is already linked to another account";
                                            }


                                        }

                                    }
                                }


                            }
                        }
                    }
                    else
                    {
                        queryRunner.CreateChannel(new ChannelModification()
                        {

                            IsTemporary = true,
                            ChannelOrder = 186,
                            MaxClients = 1,
                            Name = currentUser.Username,
                            Password = viewModel.Password
                        });
                    }

                    queryRunner.Logout();
                }
            }


            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ShowTeamspeak()
        {
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, false);
            var CurrentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (CurrentUser.ID <= 0 || CurrentUser.Permission < Models.ReturnModels.User.PermissionGroup.USER)
                return RedirectToAction("Index", "Login");



            ShowTeamSpeakModel returnModel = new ShowTeamSpeakModel();

            ListResponse<ChannelListEntry> channelList;
            ListResponse<ClientListEntry> clientList;
            List<TSChannel> AllChanels = new List<TSChannel>();

            using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
            {
                queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                queryRunner.SelectVirtualServerById(1);
                queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                { // REAL EXCECUTED CODE
                    channelList = queryRunner.GetChannelList();
                    clientList = queryRunner.GetClientList();


                }
                queryRunner.Logout();
            }

            foreach (var channelEntry in channelList)
            {
                AllChanels.Add(new TSChannel()
                {
                    ChannelName = channelEntry.Name,
                    ChannelID = channelEntry.ChannelId,
                    Order = channelEntry.Order.ToInt(),
                    ParentID = channelEntry.ParentChannelId,
                    isSpacer = channelEntry.Name.Contains("[spacer")
                });
            }

            foreach (TSChannel channel in AllChanels)
            {
                var clientsInChannel = clientList.Values.FindAll(x => x.ChannelId == channel.ChannelID);
                foreach (ClientListEntry client in clientsInChannel)
                {
                    channel.TSUsers.Add(new TSUser() { NickName = client.Nickname, isBot = (client.ClientType == 1) });
                }
            }


            foreach (TSChannel channel in AllChanels)
            {
                channel.Children.AddRange(AllChanels.FindAll(x => x.ParentID == channel.ChannelID));
            }
            TSChannel root = new TSChannel() { ChannelID = 0, ChannelName = "Permanente Call" };
            root.Children.AddRange(AllChanels.FindAll(x => x.ParentID == 0));

            returnModel.TSRootChannel = root;

            return View(returnModel);
        }

        [HttpGet]
        public ActionResult ManageUsers(int strike = -1, string disableTSUser = null, int delete = -1)
        {
            if(!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;
            var CurrentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (CurrentUser.ID <= 0 || CurrentUser.Permission <= Models.ReturnModels.User.PermissionGroup.OPERATOR)
                return RedirectToAction("Index", "Login");


            if (strike > -1)
            {
                AccountRepo.StrikeUser(strike);
                return RedirectToAction("ManageUsers");
            }
            if (disableTSUser != null)
            {
                Teamspeak.DisableTeamspeakUser(disableTSUser);
                return RedirectToAction("ManageUsers");
            }
            if (delete >-1)
            {
                AccountRepo.DeleteAccount(delete);
                return RedirectToAction("ManageUsers");
            }

            UserManagementModel returnModel = new UserManagementModel();
            
            List<User> accounts = AccountRepo.GetAllUsers();
            List<TSUser> tsUsers = TeamspeakUserRepo.GetAllTSUsers();
            foreach (var account in accounts)
            {
                DateTime strikeTime = DateTime.Now.AddMinutes(-15*Math.Pow(2, account.Strikes - 3.0).ToInt());
                account.hasBeenStriked = account.LastStrike > strikeTime;
                account.TSUsers.AddRange(tsUsers.FindAll(x => x.AccountID == account.ID));
            }

            returnModel.UserList = accounts;
            return View(returnModel);
        }

        [HttpPost]
        public ActionResult ManageUsers(UserManagementModel model= null)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;
            if (model==null) return RedirectToAction("ManageUsers");
            var CurrentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (CurrentUser.ID <= 0 || CurrentUser.Permission <= Models.ReturnModels.User.PermissionGroup.OPERATOR)
                return RedirectToAction("Index", "Login");

            if (model.UserList.Count > 0)
            {
                AccountRepo.UpdateAccount(model.UserList[0]);
                foreach (TSUser tsUser in model.UserList[0].TSUsers)
                {
                    TeamspeakUserRepo.UpdateTSUser(tsUser.TeamspeakDBID, tsUser);
                }
            }

            return RedirectToAction("ManageUsers");
        }
    }
}