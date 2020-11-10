using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database.Models;

namespace Tools.Services
{
    public interface IWowToolsService
    {
        string AddCharacter(string playerName, string name, string realm);
        IList<Character> GetAllCharacters();
        CharacterEquipmentCache GetCharacterItems(int characterId, bool newCache);
        void RemoveCharacter(int id);
    }
}
