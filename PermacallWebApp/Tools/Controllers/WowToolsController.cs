using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCDataDLL;
using Tools.Database.Models;
using Tools.Models;
using Tools.Services;

namespace Tools.Controllers
{
    public class WowToolsController : Controller
    {
        private readonly ILogger<WowToolsController> _logger;
        private readonly IWowToolsService _wowToolsService;


        public WowToolsController(ILogger<WowToolsController> logger, IWowToolsService wowToolsService)
        {
            _logger = logger;
            _wowToolsService = wowToolsService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
