using Microsoft.EntityFrameworkCore;
using PCDataDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database.Models;

namespace Tools.Database
{
    public class ToolContext : DbContext
    {
        public ToolContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterEquipmentCache> CharacterEquipmentCaches { get; set; }
        public DbSet<ItemCache> ItemCaches { get; set; }
    }
}
