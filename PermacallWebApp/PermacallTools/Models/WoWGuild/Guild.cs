using System;
using System.Collections.Generic;

namespace PermacallTools.Models.WoWGuild
{
    public class Guild
    {
        
        public List<WoWCharacter> WoWCharacters { get; set; }
        public DateTime Updated { get; set; }
        public Guild()
        {
            WoWCharacters = new List<WoWCharacter>();
        }
    }
}