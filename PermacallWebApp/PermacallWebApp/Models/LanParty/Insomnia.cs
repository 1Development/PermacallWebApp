using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.LanParty
{
    public class Insomnia
    {
        public Insomnia()
        {
            Users = new List<InsomniaUser>();
            Start = DateTime.Now;
        }

        public Insomnia(DateTime start, bool active)
        {
            Start = start;
            Active = active;
            Users = new List<InsomniaUser>();
        }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:00}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }
        public bool Active { get; set; }
        public List<InsomniaUser> Users { get; set; }
        public int LanPartyPeopleCount { get; set; }

    }
}