using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Mvc;
using PermacallWebApp.Logic;
using PermacallWebApp.Models;
using PermacallWebApp.Models.ReturnModels;
using PermacallWebApp.Models.ViewModels;
using PermacallWebApp.Repos;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Index(int id = -1, int a = 0)
        {
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, false);
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
                    using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
                    {
                        queryRunner.Login(LoginData.ServerUsername, LoginData.ServerPassword).GetDumpString();
                        queryRunner.SelectVirtualServerById(1);
                        queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                        TeamspeakUserRepo.DisableTSUser(currentUser.TSUsers[id].TeamspeakDBID);
                        queryRunner.DeleteClientFromServerGroup(9,
                            Convert.ToUInt32(currentUser.TSUsers[id].TeamspeakDBID));

                        queryRunner.Logout();
                    }
                    return RedirectToAction("Index", "Management", new { id = -1, a = 0});

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
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, false);
            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID <= 0) return RedirectToAction("Index", "Login");

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            currentUser.TSUsers = TeamspeakUserRepo.GetTeamspeakUsers(currentUser.ID);

            if(viewModel.ToChangeName != null && !currentUser.TSUsers.Any(x => x.NickName == viewModel.ToChangeName))
            {
                Repos.TeamspeakUserRepo.EditTSUser(currentUser.TSUsers[id].TeamspeakDBID,
                    new TSUser(currentUser.TSUsers[id].TeamspeakDBID, viewModel.ToChangeName, currentUser.ID));
            }

            return RedirectToAction("Index", "Management", new { id = -1});
        }

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
                    queryRunner.Login(LoginData.ServerUsername, LoginData.ServerPassword).GetDumpString();
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
    }
}