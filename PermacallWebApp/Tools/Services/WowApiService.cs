using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database;
using Tools.Database.Models;

namespace Tools.Services
{
    public class WowApiService : IWowApiService
    {
        private ToolContext toolContext;

        public WowApiService(ToolContext toolContext)
        {
            this.toolContext = toolContext;
        }

        public IList<CharacterItems> GetAllCharacterItems(IList<Character> characters)
        {
            throw new NotImplementedException();
        }

        public IList<CharacterItems> GetCharacterItems(Character characters)
        {
            throw new NotImplementedException();
        }

        public IList<Character> GetCharacters()
        {
            throw new NotImplementedException();
        }
    }
}
