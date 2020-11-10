using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PCAuthLibCore;
using Tools.Database.Models;
using Tools.Services;

namespace Tools.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class WowToolsApiController : ControllerBase
    {
        private readonly IWowToolsService _wowToolsService;

        public WowToolsApiController(IWowToolsService wowToolsService)
        {
            _wowToolsService = wowToolsService;
        }

        [HttpGet("GetAllCharacters")]
        public IActionResult GetAllCharacters()
        {
            return Ok(_wowToolsService.GetAllCharacters());
        }

        [HttpPost("AddCharacter/{playerName}/{realmslug}/{characterName}")]
        public IActionResult AddCharacter(string playerName, string realmslug, string characterName)
        {
            if (Login.GetCurrentUser(HttpContext).Permission < PCAuthLibCore.User.PermissionGroup.OPERATOR)
            {
                return Unauthorized();
            }

            return Ok(_wowToolsService.AddCharacter(playerName, characterName, realmslug));
        }

        [HttpPost("RemoveCharacter/{id}")]
        public IActionResult RemoveCharacter(int id)
        {
            if (Login.GetCurrentUser(HttpContext).Permission < PCAuthLibCore.User.PermissionGroup.OPERATOR)
            {
                return Unauthorized();
            }
            _wowToolsService.RemoveCharacter(id);
            return Ok();
        }

        [HttpGet("GetCharacterItems/{characterId}")]
        public IActionResult GetCharacterItems(int characterId)
        {
            return Ok(_wowToolsService.GetCharacterItems(characterId, false));
        }

        [HttpGet("GetNewCharacterItems/{characterId}")]
        public IActionResult GetNewCharacterItems(int characterId)
        {
            return Ok(_wowToolsService.GetCharacterItems(characterId, true));
        }

        //// GET: api/WowToolsApi
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/WowToolsApi/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/WowToolsApi
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/WowToolsApi/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
