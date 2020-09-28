using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database.Models;

namespace Tools.Services
{
    public interface IWowApiService
    {
        public IList<Character> GetCharacters();
        public IList<CharacterItems> GetAllCharacterItems(IList<Character> characters);
        public IList<CharacterItems> GetCharacterItems(Character characters);
    }
}
