using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ReturnModels
{
    public class TSChannel
    {
        public string ChannelName { get; set; }
        public uint ChannelID { get; set; }
        public List<TSChannel> Children { get; set; }
        public List<TSUser> TSUsers { get; set; }
        public uint ParentID { get; set; }
        public int Order { get; set; }
        public bool isSpacer { get; set; }

        public TSChannel()
        {
            TSUsers = new List<TSUser>();
            Children = new List<TSChannel>();
        }
    }
}