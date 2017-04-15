using System.Collections.Generic;

namespace PermacallWebApp.Models
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