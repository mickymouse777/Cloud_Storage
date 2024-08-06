using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cloud_Storage.Models;

namespace Cloud_Storage.Data
{
    public class Cloud_StorageContext : DbContext
    {
        public Cloud_StorageContext (DbContextOptions<Cloud_StorageContext> options)
            : base(options)
        {
        }

        public DbSet<Cloud_Storage.Models.Bird> Bird { get; set; } = default!;
        public DbSet<Cloud_Storage.Models.Birder> Birder { get; set; } = default!;
        public DbSet<Cloud_Storage.Models.Sighting> Sighting { get; set; } = default!;
    }
}
