using System.Collections.Generic;

namespace PermacallWebApp.Models.LanParty
{
    public class LanParty
    {
        public LanParty()
        {
            LanPartyInsomnia = new Insomnia();
        }

        public LanParty(int id, string lanPartyName, string lanPartyContent, Insomnia insomnia)
        {
            ID = id;
            LanPartyName = lanPartyName;
            LanPartyContent = lanPartyContent;
            LanPartyInsomnia = insomnia;
        }

        public int ID { get; set; }
        public string LanPartyName { get; set; }
        public string LanPartyContent { get; set; }
        public Insomnia LanPartyInsomnia { get; set; }

        public int Owner { get; set; }
        public string NewDropOut { get; set; }
        public bool FullScreen { get; set; }
    }
}