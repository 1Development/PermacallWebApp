using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Models
{
    public class AddCharacterModel
    {
        public string Player { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public bool IsMain { get; set; }
    }
}
