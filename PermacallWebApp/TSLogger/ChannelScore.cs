using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSLogger
{
    public class ChannelScore
    {
        public ChannelScore(string name, uint id, ulong score)
        {
            Name = name;
            Id = id;
            Score = score;
        }

        public ChannelScore(string name, uint id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public uint Id { get; set; }
        public double Score { get; set; }

    }
}
