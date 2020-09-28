using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database.Models;

namespace Tools.Database
{
    public class ToolContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterItems> CharacterItems { get; set; }
    }
}
